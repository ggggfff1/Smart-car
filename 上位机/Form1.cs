using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Timers;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace 上位机
{
    
    public partial class Form1 : Form
    {
        public Form1()//构造函数
        {
            InitializeComponent();


            #region 调色盘
            ColorPalette cp = bm.Palette;
            cp.Entries[0] = Color.White;
            cp.Entries[1] = Color.Black;
            cp.Entries[2] = Color.Red;//new
            cp.Entries[3] = Color.Green;
            cp.Entries[4] = Color.Blue;
            cp.Entries[5] = Color.Yellow;
            bm.Palette = cp;
            #endregion

            //初始化数组
            for (int i = 0; i < height; i++)
                img[i] = new byte[width];

            //定时器
            t.Interval = 40;
            t.AutoReset = true;
            t.Elapsed += new ElapsedEventHandler(delegate
            {
                if (playFlag)
                    ReadPic();
            });

            MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;//设置窗口最大化为屏幕工作区
            WindowState = FormWindowState.Maximized;//最大化显示
        }



        private void Form1_SizeChanged(object sender, EventArgs e)//大小改变时，保持panel1的width/height不变
        {
            splitContainer1.SplitterDistance = 100;
            splitContainer2.SplitterDistance = (int)(splitContainer2.Panel1.Height * width / (float)height);
        }



        private void pbPath_Click(object sender, EventArgs e)//文件夹图标点击事件
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbPath.Text = ofd.FileName;//保存文件名
            }
        }





        FileStream fs;//文件流
        byte[] buffer = new byte[1024];//读取文件缓冲区
        bool playFlag = false, openFlag = false;//播放标志，文件打开标志
        int frameCount = 0;//帧数计数
        System.Timers.Timer t = new System.Timers.Timer();//定时器

        private void btStart_Click(object sender, EventArgs e)//开始按钮按下
        {
            try
            {
                if (btStart.Text == "开始")
                {
                    fs = File.OpenRead(tbPath.Text);
                    t.Start();//开启定时器
                    btStart.Text = "停止";
                    openFlag = true;
                    playFlag = true;
                    frameCount = 0;
                    fs.Position = 0;
                }
                else if (btStart.Text == "停止")
                {
                    t.Stop();//关闭定时器
                    fs.Close();//关闭文件
                    btStart.Text = "开始";
                    openFlag = false;
                    playFlag = false;

                }
            }
            catch (Exception ee)
            {
                if (fs != null)
                {
                    fs.Close();//关闭文件
                    fs = null;
                }
                MessageBox.Show(ee.Message);
            }
        }



 
        const int height = 50, width = 120, imgSize = width * height / 8;//图像宽高
        byte[][] img = new byte[height][];//读取图像缓冲区
        byte[] img2 = new byte[width * height];//拷贝到bitmap的图像缓冲区
        byte[] mask = { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };//处理图像时的掩模
        Bitmap bm = new Bitmap(width, height, PixelFormat.Format8bppIndexed);//要显示的图像
        BitmapData bmd;//图像数据
        delegate void Method();//用于Invoke的函数

        private void ReadPic()//读取并显示图像
        {
            if (openFlag)
            {
                try
                {
                    #region 读取图像
                    if (fs.Read(buffer, 0, 1024) == 1024)//读取文件
                        frameCount++;//帧计数加一
                    else
                        return;

                    int num = 0;

                    for (int i = 0; i < height; i++)//1位转8位
                    {
                        for (int j = 0; j < width;)
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                if ((buffer[num] & mask[k]) == 0)
                                    img[i][j++] = 1;  
                                else
                                    img[i][j++] = 0;   
                            }
                            num++;
                        }
                    }
                    #endregion


                    #region 图像处理
                    ImgProc(img);
                    #endregion


                    #region 图像显示
                    
                    num = 0;
                    for (int i = 0; i < height; i++)//二维转一维
                        for (int j = 0; j < width; j++)
                        {
                            img2[num] = img[i][j];
                            num++;
                        }

                    bmd = bm.LockBits(new Rectangle(0, 0, width, height),
                        ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);//锁定图像
                    Marshal.Copy(img2, 0, bmd.Scan0, imgSize * 8);
                    bm.UnlockBits(bmd);


                    Invoke(new Method(delegate
                    {
                        lbFrame.Text = String.Format("第{0}帧", frameCount);//显示帧数
                        pictureBox1.Image = (Bitmap)bm.Clone();//显示图片
                        myone.Text = (leftupY).ToString();
                        mytwo.Text = (leftdownY).ToString();
                        mythree.Text = (oo).ToString();
                        myfour.Text = (fuck).ToString();
                    }));
                    #endregion
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message + " " + ee.StackTrace);
                }
            }
        }




        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)//按键响应
        {
            if (keyData == Keys.Space)//空格暂停和播放
            {
                if (btStart.Text == "开始")
                    btStart_Click(null, null);
                else
                    playFlag = !playFlag;

                return true;//表明该事件已被处理
            }
            else if (keyData == Keys.Left)//左光标退一帧
            {
                if (openFlag)
                    if (!playFlag)
                        if (fs.Position >= 2048)
                        {
                            fs.Position -= 2048;
                            frameCount -= 2;
                            ReadPic();
                        }

                return true;
            }
            else if (keyData == Keys.Right)//右光标前进一帧
            {
                if (openFlag)
                    if (!playFlag)
                        ReadPic();

                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);

        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)//窗体关闭事件
        {
            if (fs != null)
            {
                fs.Close();//关闭文件
                fs = null;
            }
        }

        void empty(int[] flag, int value, byte number)//用于清空数组
        {
            for (byte i = 0; i < number; i++)
            {
                flag[i] = value;
            }
        }


        /*********************************************************************************************************************
                                                                 程序开始
        *********************************************************************************************************************/
        #region 参数
        /*********************************************************************************************************************
                                                                  全局参数
         *********************************************************************************************************************/
        static int imgheight = 50;//图像高度
        static int imgwidth = 120;//图像宽度
        int Black = 1;//黑色
        int White = 0;//白色
        int xmiddle = (0 + imgwidth) / 2;//图像的中点
        int leftdirection = 1;//决定方向，向左的方向
        int rightdirection = 0;//决定方向，向右的方向
        int leftupY = 0;
        int leftdownY = 0;
        int rightupY = 0;
        int rightdownY = 0;
        int leftcompare = 0;
        int rightcompare = 0;
        int trustline = 0;
        bool yy;
        double oo;
        double oo2;
        string fuck;
        /*********************************************************************************************************************
                                                                  搜线参数
         *********************************************************************************************************************/
        int firstlose_left = 0;//左边第一次丢线行
        int firstlose_right = 0;//右边第一次丢线行
        int finalloseline_right = 0;//右边最终丢线行
        int finalloseline_left = 0;//左边最终丢线行
        int finalloseline = 0;//最终有效行
        int loserightflag = 0;//判断丢线 1是有丢线
        int loseleftflag = 0;//判断丢线 1是有丢线
        int[] boundary_left = new int[imgheight];//左边界横坐标
        int[] boundary_right = new int[imgheight];//右边界横坐标
        int[] middle_line = new int[imgheight];//中线横坐标        
        int[] lost_right_flag = new int[imgheight];	//记录右边丢线行，丢线为1，否则为0
        int[] lost_left_flag = new int[imgheight]; //记录左边丢线行，丢线为1，否则为0
        #endregion


        /*********************************************************************************************************************
                                                          最小二乘法
        *输入：downy：起始纵坐标；upy：结束纵坐标（downy>upy）；array：要计算的数组
        *返回；斜率
        *思路：
        *********************************************************************************************************************/
        #region 最小二乘法
        float leastSquareLinearFit(int downy, int upy,int[] array)
        {
            float k;
            int i, count;
            int num = downy - upy - 1;//求解的是有效行以下的点的斜率
            float sum_x2 = 0.0f;
            float sum_y = 0.0f;
            float sum_x = 0.0f;
            float sum_xy = 0.0f;
            count = 0;
            for (i = upy + 1; i < downy; i++)
            {
                sum_x2 += array[i] * array[i];
                sum_y += i;
                sum_x += array[i];
                sum_xy += array[i] * i;
                if (array[i] == array[downy])
                {
                    count++;
                }
            }
            if (count >= 0.95 * num)//当计算的直线大部分横坐标都一样时，斜率为无穷大，直接返回一个较大值
            {
                k = 10;
            }
            else
            {
                if ((num * sum_x2 - sum_x * sum_x) != 0)//防止分母为0
                {
                    k = (num * sum_xy - sum_x * sum_y) / (num * sum_x2 - sum_x * sum_x);
                }
                else
                {
                    k = 10;
                }

            }
            return k;
        }
        #endregion
        /*********************************************************************************************************************
                                                                  清空数组
         *输入：flag：要被清空的数组；value：每个数组要赋的值；start：起始点；end：终点（end>start）
         *返回；无
         *思路：在选定范围内遍历数组，将相应的值设为要赋的值
         *********************************************************************************************************************/
        #region 清空数组
        void empty(int[] flag, int value, int start, int end)//用于清空数组
        {
            for (int i = start; i < end; i++)
            {
                flag[i] = value;
            }
        }
        #endregion

        /*********************************************************************************************************************
                                                                 逐行搜跳变函数
         *输入：startx:搜线的起始横坐标；direction:搜线的方向；row:要在第几行搜线；change:黑或白，黑到白的跳变（White）还是白到黑的跳变（Black）,
         *      fail_boundary:当找不到符合的边界时把边界设定的值；left:跳变左边需要有几个同颜色点；right:跳变右边需要有几个同颜色点
         *解释：left、right：用来决定搜到白白黑黑还是白黑还是白黑黑等，才算是跳变
         *      fail_boundary：一个应用例子：找有边界时，如果没有符合条件的跳变点，把该值设为119，则该行的边界就为119
         *返回；搜到跳变列的横坐标
         *思路：从起始点开始，往选定的方向搜索跳变点，返回搜到边线的横坐标
         *********************************************************************************************************************/
        #region 逐行搜跳变函数
        int findchange(int startx, int direction, int row, int change, int fail_boundary, int left, int right)
        {
            int j, i;
            bool leftachieve = true;//判断左边是否达到连续点的条件
            bool rightachieve = true;//判断右边是否达到连续点的条件
            if (direction == 0)//右边
            {
                for (j = startx; j < imgwidth - right - 1; j++) //先找到有跳变
                {
                    leftachieve = true;
                    rightachieve = true;
                    if (img[row][j + 1] == change)//j为跳变前的点，j+1为跳变后的点
                    {
                        for (i = j + 1; i < j + 1 + right; i++)//右边满足有连续right个change颜色的点
                        {
                            if (img[row][i] != change)
                            {
                                rightachieve = false;
                                break;
                            }
                        }
                        if (!rightachieve)
                        {
                            continue;
                        }
                        else
                        {
                            for (i = j; i > j - left; i--)//右边满足条件的情况下，判断左边是否有连续left个和change颜色相反的点
                            {
                                if (img[row][i] != (1 - change))
                                {
                                    leftachieve = false;
                                    break;
                                }
                            }
                        }
                        if (!leftachieve)
                        {
                            continue;
                        }
                        else
                        {
                            return j;//如果两个条件都符合，返还找到的边界点
                        }
                    }
                }
                return fail_boundary;
            }
            else//左边，思路和右边一样
            {
                for (j = startx; j > left; j--)
                {
                    leftachieve = true;
                    rightachieve = true;
                    if (img[row][j - 1] == change)//j为跳变前的点，j-1为跳变后的点
                    {
                        //Console.WriteLine(j);
                        for (i = j - 1; i > j - 1 - left; i--)
                        {
                            if (img[row][i] != change)
                            {
                                leftachieve = false;
                                break;
                            }
                        }
                        if (!leftachieve)
                        {
                            continue;
                        }
                        else
                        {
                            for (i = j; i < j + right; i++)
                            {
                                if (img[row][i] != (1 - change))
                                {
                                    rightachieve = false;
                                    break;
                                }
                            }
                            if (!rightachieve)
                            {
                                continue;
                            }
                            else
                            {
                                return j;
                            }
                        }
                    }
                }
                return fail_boundary;
            }
        }
        #endregion
        /*********************************************************************************************************************
                                                                 判断丢线函数
         *输入：direction：判断的方向；arrayfind：需要找丢线情况的数组；row：要在第几行寻找丢线情况
         *返回；丢线情况1为丢线，0为不丢线
         *概念：丢线就是搜到的边界横坐标为图像的边界
         *思路：若某行搜到边界的横坐标为图像的边界，则返回1
         *********************************************************************************************************************/
        #region 判断丢线函数
        int findlose(int direction, int[] arrayfind, int row)
        {
            if (direction == 0)
            {
                if (arrayfind[row] == imgwidth - 1)//图像宽度为imgwidth，但在数组中的索引值应该减一
                {
                    return 1;
                }
            }
            else
            {
                if (arrayfind[row] == 0)
                {
                    return 1;
                }

            }
            return 0;
        }
        #endregion
        /*********************************************************************************************************************
                                                                 中线调整函数
         *输入：startx：起始点横坐标；row：要在第几行调整，
         *返回；调整后的中点横坐标
         *思路：若起始点为黑色，则对中点调整，从原中点往两边找赛道，对比左边找到的中点和右边找到的中点，哪一个离原来的中点近
         *      就把中点调整过去
         *********************************************************************************************************************/
        #region 中线调整函数
        int middleadjust(int startx, int row)
        {
            int adjustedleft = 0;//调整后左边界点
            int adjustedright = 0;//调整后右边界点
            int left_distance = 0;//左边调整的距离
            int right_distance = 0;//右边调整的距离
            adjustedright = findchange(startx, rightdirection, row, Black, (imgwidth - 1), 1, 2);//找右边可调整点，findchange是逐行搜跳变函数值
            adjustedleft = findchange(startx, leftdirection, row, Black, 0, 2, 1);//找左边可调整点
            if (adjustedright == (imgwidth - 1) && adjustedleft == 0)//没有找到可以调整的点，返回原值
            {
                return startx;
            }
            left_distance = startx - adjustedleft;//计算两边调整的值的大小
            right_distance = adjustedright - startx;
            if (left_distance <= right_distance)//取调整小的作为调整后的中点，返回
            {
                return adjustedleft;
            }
            else
            {
                return adjustedright;
            }
        }
        #endregion

        /*********************************************************************************************************************
                                                                 搜边线函数
         *输入：firstx：起始点横坐标；downy：起始y坐标；upy：结束的y坐标（down>up）；iffindlose：是否找丢线；ifadjust：是否调整中线；
         *      bleft:要存储数据的左边数组；bright:要存储数据的右边数组；leftsize:左边数组的大小；rightsize:右边数组的大小
         *解释：一个例子：找边界时，把boundary_left传给bleft，boundary_right传给bright
         *返回；
         *思路：在自己想要的范围寻找边线
         *********************************************************************************************************************/
        #region 搜边线函数
        void findboundary(int firstx, int downy, int upy, bool iffindlose, bool ifadjust, int[] bleft, int[] bright, int leftsize, int rightsize)
        {
            empty(bleft, 0, 0, leftsize);//初始化数组
            empty(bright, 0, 0, rightsize);
            if (iffindlose)
            {
                empty(lost_right_flag, 0, 0, imgheight);
                empty(lost_left_flag, 0, 0, imgheight);
            }
            int i;
            middle_line[downy] = firstx;//规定起始行的横坐标
            for (i = downy; i >= upy; i--)
            {
                bleft[i] = findchange(middle_line[i], leftdirection, i, Black, 0, 1, 0);//逐行搜边线
                bright[i] = findchange(middle_line[i], rightdirection, i, Black, imgwidth - 1, 0, 1);
                if (iffindlose)//对每行寻找丢线情况
                {
                    lost_left_flag[i] = findlose(leftdirection, bleft, i);
                    lost_right_flag[i] = findlose(rightdirection, bright, i);
                }
                middle_line[i] = (bleft[i] + bright[i]) / 2;//确定该行的中点，作为i-1行的起始点
                if (i != upy)
                {
                    if (img[i - 1][middle_line[i]] == Black)//i-1行的起始点为黑色时，把起始点设为图像中点
                    {
                        if (img[i - 1][xmiddle] == Black && ifadjust)//图像中点也为黑色，且开启了中点调整后，执行调整中点的函数
                        {
                            middle_line[i - 1] = middleadjust(xmiddle, i - 1);
                        }
                        else
                        {
                            middle_line[i - 1] = xmiddle;
                        }

                    }
                    else//起始点不为黑色时，直接将i行的中点作为i-1行的起始点
                    {
                        middle_line[i - 1] = middle_line[i];
                    }
                }
            }


        }

        #endregion
        /*********************************************************************************************************************
                                                                 显示列和行
         *输入：downy：开始点；upy：结束点；要显示的数组或者行，颜色
         *返回；
         *思路：showcolarray:将某个按列存储的数组（例如左右边界）显示出来。showcol：显示某一列。showrow：显示某一行
         *********************************************************************************************************************/
        #region 显示
        void showcolarray(int downy, int upy, int[] array, byte colour)
        {
            int i;
            for (i = downy; i >= upy; i--)
            {
                img[i][array[i]] = colour;
            }
        }
        void showcol(int downy, int upy, int col, byte colour)
        {
            int i;
            for (i = downy; i >= upy; i--)
            {
                img[i][col] = colour;
            }
        }
        void showrow(int startx, int endx, int row, byte colour)
        {
            int i;
            for (i = endx; i >= startx; i--)
            {
                img[row][i] = colour;
            }
        }
        #endregion

        /*********************************************************************************************************************
                                                                寻找第一次丢线行函数
         *输入：downy：开始点；upy：结束点；direction：方向
         *返回；找到的第一次丢线行的纵坐标
         *思路：在一定范围内寻找lost_right_flag或者lost_left_flag数组中第一次出现1对应的索引值
         *********************************************************************************************************************/
        #region 寻找第一次丢线行函数
        int findfirstlose(int downy, int upy, int direction)
        {
            int i;
            if (direction == 0)
            {
                for (i = downy; i >= upy; i--)
                {
                    if (lost_right_flag[i] == 1 && img[i][middle_line[i]] == White)
                    {
                        return i;
                    }
                }
            }
            else
            {
                for (i = downy; i >= upy; i--)
                {
                    if (lost_left_flag[i] == 1 && img[i][middle_line[i]] == White)
                    {
                        return i;
                    }
                }
            }
            return -1;//没有丢线

        }
        #endregion

        /*********************************************************************************************************************
                                                                寻找最终丢线行函数
         *输入：firstlosey：开始点；upy：结束点；direction：方向
         *返回；找到的左边或者右边连续最后一次丢线行的纵坐标
         *思路：从第一次丢线开始往往上找连续丢线行
         *********************************************************************************************************************/
        #region 寻找最终丢线行函数
        int findfinallose(int firstlosey, int upy, int direction)
        {
            int i;
            if (direction == 0)
            {
                i = firstlosey;
                while (lost_right_flag[i] == 1 && img[i][middle_line[i]] == White)
                {
                    i--;
                    if (i < upy)//最终丢线行不能超过上限值
                    {
                        i++;
                        break;
                    }
                }
                return i;
            }
            else
            {
                i = firstlosey;
                while (lost_left_flag[i] == 1 && img[i][middle_line[i]] == White)
                {
                    i--;
                    if (i < upy)
                    {
                        i++;
                        break;
                    }
                }
                return i;
            }
        }
        #endregion
        /*********************************************************************************************************************
                                                                确定丢线行函数
         *输入：firstlosedown：找第一次丢线时寻找范围的下边界值；firstloseup：找第一次丢线时寻找范围的上边界值；
         *      finalloseup：找左和右最终丢线行时寻找范围的上边界值
         *返回；
         *思路：
         *********************************************************************************************************************/
        #region 确定丢线行函数
        void findloseline(int firstlosedown, int firstloseup, int finalloseup)
        {
            firstlose_left = 0;
            firstlose_right = 0;
            finalloseline_right = 0;
            finalloseline_left = 0;
            finalloseline = 0;
            firstlose_left = findfirstlose(firstlosedown, firstloseup, leftdirection);
            firstlose_right = findfirstlose(firstlosedown, firstloseup, rightdirection);
            if (firstlose_left == -1)
            {
                loseleftflag = 0;//左边没有丢线
            }
            else
            {
                loseleftflag = 1;
                finalloseline_left = findfinallose(firstlose_left, finalloseup, leftdirection);
            }
            if (firstlose_right == -1)
            {
                loserightflag = 0;//右边没有丢线
            }
            else
            {
                loserightflag = 1;
                finalloseline_right = findfinallose(firstlose_right, finalloseup, rightdirection);
            }
            if (finalloseline_right >= finalloseline_left)
            {
                finalloseline = finalloseline_right;
            }
            else
            {
                finalloseline = finalloseline_left;
            }
            if (loseleftflag == 1 && firstlose_left < finalloseline)
            {
                loseleftflag = 0;
            }
            if (loserightflag == 1 && firstlose_right < finalloseline)
            {
                loserightflag = 0;
            }
        }
        #endregion
        /*********************************************************************************************************************
                                                              右边补线函数
        *输入：  upy   图像下方点的y值；   downy 图像上方点的y值
        *返回；
        *思路：从下方特征点所在行开始，for循环计算到上方特征点所在行，找出竖坐标满足线段表达式的点，并赋上颜色
        *********************************************************************************************************************/
        #region 右边补线函数
        void rightcalculate(int upy, int downy)
        {
            double kk=0.0;
            int i;
           kk = (downy - upy) / 1.0/(boundary_right[downy] - boundary_right[upy]);
            
            for (i = upy - 1; i > downy; i--)
            {
                boundary_right[i] = (int)(((i - upy) / kk) + boundary_right[upy]);
                if (boundary_right[i] > imgwidth - 1) boundary_right[i] = imgwidth - 1;
                if (boundary_right[i] < 0) boundary_right[i] = 0;
            }
            oo2 = kk;

        }
        #endregion

        /*********************************************************************************************************************
                                                              左边补线函数
        *输入：  upy   图像下方点的y值；   downy 图像上方点的y值
        *返回；
        *思路：从下方特征点所在行开始，for循环计算到上方特征点所在行，找出竖坐标满足线段表达式的点，并赋上颜色
        *********************************************************************************************************************/
        #region   左边补线函数
        void leftcalculate(int upy, int downy)
        {
            double kk = 0.0;
            int i;
            kk = (downy - upy) /1.0/ (boundary_left[downy] - boundary_left[upy]);
            
            for (i = upy - 1; i > downy; i--)
            {
                boundary_left[i] = (int)(((i - upy) / kk) + boundary_left[upy]);
                if (boundary_left[i] > imgwidth - 1) boundary_left[i] = imgwidth - 1;
                if (boundary_left[i] < 0) boundary_left[i] = 0;
            }
            oo = kk;

        }
        #endregion


        /*********************************************************************************************************************
                                                               补线函数
        *输入：  
        *返回；
        *思路：从下方特征点所在行开始，for循环计算到上方特征点所在行，找出竖坐标满足线段表达式的点，并赋上颜色
        *********************************************************************************************************************/
        #region 补线

        #endregion

        /*********************************************************************************************************************
                                                              搜索边界！！
       *输入：  downy，最下面的y值   upy 最上面的y值   bleft左边边界的数组   bright右边界的数组   leftsize 左边数组长度
       *         rightsize 右边数组的长度   firstx就是开始定义的x值
       *返回；
       *思路：搜索边界，并且根据情况调用边界处理函数，然后求出特征点，进行布线，是总的补线函数
       *********************************************************************************************************************/
        #region   搜索边界

        void myfindline(int downy, int upy, int[] bleft, int[] bright, int leftsize, int rightsize, int firstx)
        {
            leftcompare = 0;
            rightcompare = 0;
            empty(boundary_left, 0, 0, leftsize);//初始化数组
            empty(boundary_right, 0, 0, rightsize);
            //empty(bleft, 0, 0, leftsize);//初始化数组
            //empty(bright, 0, 0, rightsize);
            middle_line[upy] = firstx;
            trustline = 0;
            for (int i = upy; i >= downy + 1; i--)
            {               
                bleft[i] = findchange(middle_line[i], leftdirection, i, Black, 0, 1, 0);//逐行搜边线
                bright[i] = findchange(middle_line[i], rightdirection, i, Black, imgwidth - 1, 0, 1);
                
                if (bleft[i] == 0) leftcompare = 1;
                if (bright[i] == 119) rightcompare = 1;

                middle_line[i - 1] = abs((bleft[i] + bright[i]) / 2);//确定该行的中点，作为i-1行的起始点     
                if (i != upy)
                {          
                    if (img[i - 1][middle_line[i]] == Black)//i-1行的起始点为黑色时，把起始点设为图像中点
                    {
                        if (img[i - 1][xmiddle] == Black)//图像中点也为黑色，且开启了中点调整后，执行调整中点的函数
                        {
                            middle_line[i - 1] = middleadjust(xmiddle, i - 1);
                        }
                        else
                        {
                            middle_line[i - 1] = xmiddle;
                        }
                    }
                    //else//起始点不为黑色时，直接将i行的中点作为i-1行的起始点
                    //{
                    //    middle_line[i - 1] = middle_line[i];
                    //}
                }
              //  if (abs(middle_line[i - 1]) > 7 + middle_line[i]) middle_line[i - 1] = middle_line[i];  //这里可以调整参数              
            }
            for (int i = 15; i > 0; i--)//搜索可信线
            {
                if (bleft[i] > bright[i - 1] || bright[i] < bleft[i - 1])
                {
                    trustline = i + 2;
                    break;
                }
                if (boundary_left[i] == boundary_right[i])
                {
                    trustline = i + 2;
                    break;
                }
            }

            
            
           



        }
        #endregion
        /*********************************************************************************************************************
                                                            处理右边边界！！
     *输入：  range 跳变的判断范围  deadline 可信线的y值
     *返回；
     *思路：在原始边界函数中找出特征点，之后根据特征点的位置和数量来进行补线，是核心函数
     *********************************************************************************************************************/
        #region 处理右边边界
        void rightdealboundary(int range, int deadline)
        {
            bool compare;
            rightdownY = 0;
            rightupY = 0;
            if(boundary_right[imgheight - 2] == (imgwidth - 1))
            {
                yy = true;
                compare = false;
            }
            else
            {
                yy = false;
                compare = true;
            }
            deadline = deadline >= 3 ? deadline : 3;

            for (int i = imgheight - 1; i > deadline + 2; i--)
            {


                if (yy == false)//判断下面的
                {
                    if ((boundary_right[i] + range < boundary_right[i - 1]) && (boundary_right[i] + range < boundary_right[i - 2]))  //
                    {
                        compare = false;
                        yy = true;
                        rightdownY = i + 1;
                    }
                }
                if (yy == true)//判断上面的
                {
                    if (boundary_right[i] > boundary_right[i - 1])//跳变
                    {
                        compare = true;
                    }
                    if (abs(boundary_right[i - 1] - boundary_right[i - 2]) <= 5 && (abs(boundary_right[i - 2] - boundary_right[i - 3]) <= 5) && compare == true)//连续两个不跳变
                    {
                        rightupY = i - 2;
                        break;
                    }

                }
            }
        }
        #endregion

        /*********************************************************************************************************************
                                                                处理左边边界！！
     *输入：  range 跳变的判断范围  deadline 可信线的y值
     *返回；
     *思路：在原始边界函数中找出特征点，之后根据特征点的位置和数量来进行补线，是核心函数
          *********************************************************************************************************************/
        #region  处理左边边界
        void leftdealboundary(int range, int deadline)
        {
            bool compare;
            leftdownY = 0;
            leftupY = 0;
            if (boundary_left[imgheight - 2] == 0 )
            {
                yy = true;
                compare = false;
            }
            else
            {
                yy = false;
                compare = true;
            }
            deadline = deadline >= 3 ? deadline : 3;

            for (int i = imgheight - 1; i > deadline + 2; i--)
            {
                if (yy == false)//判断下面的
                {
                    if ((boundary_left[i] - range > boundary_left[i - 1]) && (boundary_left[i] - range > boundary_left[i - 2]))  //
                    {
                        compare = false;
                        yy = true;
                        leftdownY = i + 1;
                    }
                }
                if (yy == true)//判断上面的
                {
                    if (boundary_left[i] < boundary_left[i - 1])//跳变
                    {
                        compare = true;
                    }
                    if (abs(boundary_left[i - 1] - boundary_left[i - 2]) <= 5 && (abs(boundary_left[i - 2] - boundary_left[i - 3]) <= 5) && compare == true)//连续两个不跳变
                    {
                        leftupY = i - 2;
                        break;
                    }

                }
            }
            

        }


        #endregion


        /*********************************************************************************************************************
                                                           回归直线！！
    *输入：  yy  判断回归直线的边界点的y值    direction  左边界还是右边界，左边界是1，右边界是0；  times回归直线参考
    *            的点的数目
    *返回；
    *思路：最小二乘法
    *********************************************************************************************************************/
        #region  回归直线
        void regressionline(int yy, int direction, int times)
        {
            float averagex = 0;
            float averagey = 0;
            float b = 0;
            float upnumber = 0;
            float downnumber=0;
            if(direction == 1 &&   yy - times > 0)//左边
            { 
               for(int i = yy; i > yy - times; i--)
                {
                    averagex += boundary_left[i];
                    averagey += i;
                }
                averagex = averagex / 1.0f/times;
                averagey = averagey / 1.0f/times;
                for (int i = yy; i > yy - times; i--)
                {
                    upnumber += (boundary_left[i] - averagex) * (i - averagey);
                    downnumber += (boundary_left[i] - averagex) * (boundary_left[i] - averagex);
                }
                b = upnumber / 1.0f/ downnumber;
                if (b < 1000)
                {
                    for (int i = yy + 1; i <= imgheight - 1; i++)
                    {
                        boundary_left[i] = (int)((i - yy) / (b) + boundary_left[yy]);
                        if (boundary_left[i] > imgwidth - 1) boundary_left[i] = imgwidth - 1;
                        if (boundary_left[i] < 0) boundary_left[i] = 0;
                    }
                }
            }

            if (direction == 0 &&  yy - times > 0)//右边
            {
                for (int i = yy; i > yy - times; i--)
                {
                    averagex += boundary_right[i];
                    averagey += i;
                }
                averagex = averagex /1.0f/ times;
                averagey = averagey /1.0f/ times;
                for (int i = yy; i > yy - times; i--)
                {
                    upnumber += (boundary_right[i] - averagex) * (i - averagey);
                    downnumber += (boundary_right[i] - averagex) * (boundary_right[i] - averagex);
                }
                b = upnumber / 1.0f/ downnumber;
                if (b < 1000)
                {
                    for (int i = yy + 1; i <= imgheight - 1; i++)
                    {
                        boundary_right[i] = (int)((i - yy) / (b) + boundary_right[yy]);
                        if (boundary_right[i] > imgwidth - 1) boundary_right[i] = imgwidth - 1;
                        if (boundary_right[i] < 0) boundary_right[i] = 0;
                    }
                }
            }
        }




        #endregion

        /*********************************************************************************************************************
                                                           出示点！！
    *输入：  colara  左特征点的颜色；   colarb 右特征点的颜色
    *返回；
    *思路：添加颜色
    *********************************************************************************************************************/
       #region  出示点
        void showpoint(byte colora, byte colorb)
        {
            img[leftupY][boundary_left[leftupY]] = colora;
            img[leftdownY][boundary_left[leftdownY]] = colora;
            img[rightupY][boundary_right[rightupY]] = colorb;
            img[rightdownY][boundary_right[rightdownY]] = colorb;
        }


        #endregion

        /*********************************************************************************************************************
                                                               绝对值！！
    *输入：  a 整形变量
    *返回；整形绝对值常量
    *思路：三目运算符
    *********************************************************************************************************************/
        #region  绝对值

        int abs(int a)
        {
            int b;
            b = (a >= 0) ? (a) : (-a);
            return b;
        }
        #endregion

        /*********************************************************************************************************************
                                                       图像处理总函数！！
*输入：  a 整形变量
*返回；整形绝对值常量
*思路：三目运算符
*********************************************************************************************************************/
        #region 总函数
        void oneforall()
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();  //开始监视代码运行时间
                            //需要测试的代码
         
            fuck = "Null";
            myfindline(0, 49, boundary_left, boundary_right, imgheight, imgheight, 60);
            turning(trustline);
            crossroad();


            for (int i = 49; i >= trustline + 1; i--)
            {
                middle_line[i] = (boundary_left[i] + boundary_right[i]) / 2;
            }
            watch.Stop();  //停止监视
            TimeSpan timespan = watch.Elapsed;
            oo = timespan.TotalMilliseconds;
        }

        #endregion

        /*********************************************************************************************************************
                                                     十字路口判断函数！！
*输入：  a 整形变量
*返回；整形绝对值常量
*思路：三目运算符
*********************************************************************************************************************/
        #region
        bool crossroad()
        { 
            if (leftcompare == 1 && rightcompare == 1)
            {
                leftdealboundary(3, trustline);
                rightdealboundary(3, trustline);
                if (leftupY != 0 && rightupY != 0)
                {
                    if (leftdownY != 0 && rightdownY != 0)
                    {
                        leftcalculate(leftdownY, leftupY);
                        rightcalculate(rightdownY, rightupY);
                        fuck = "十字";
                        return true;
                    }
                    else if (leftdownY == 0 && rightdownY == 0)
                    {
                        regressionline(rightupY, 0, 3);
                        regressionline(leftupY, 1, 3);
                        fuck = "上半十字";
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
        #endregion

        /*********************************************************************************************************************
                                                            大弯道判断函数！！
     *输入： downY函数处理的下界
     *返回；是否为大转弯
     *思路：利用一侧多次是边边而同时另一侧会不断减少，如果同时满足这个条件就是大弯道
     *        函数中同时规定了   jj和qq变量   多少次满足以上条件才算是大弯道 
     *        同时利用 例如“左边的某个点的上面的点是黑点”来判断可信线
     *        
     *********************************************************************************************************************/
        #region
            bool turning(int downY)
        {
            int compare = 0;
            int jj = 0;
            int qq = 0;
            for(int i = imgheight - 1; i > (downY + 3); i= i - 4 )
            {
                if (boundary_left[ i] < boundary_left[ i - 3] && boundary_right[ i] == imgwidth - 1) jj++;
                if (jj == 5)
                {
                    compare = 1;// 右转
                    break;
                }
                if (boundary_right[ i] > boundary_right[ i - 3] && boundary_left[ i] == 0) qq++;
                if (qq == 5)
                {
                    compare = 2; // 左转
                    break;
                }
            }
            if(compare ==1)
            {
                for (int i = imgheight - 10; i > downY; i --)
                {
                    if (img[i - 1][boundary_right[i]] == Black) trustline = i;
                }
                    fuck = "大右转";
                
                return true;
            }
            if (compare == 2)
            {
                fuck = "大左转";
                for (int i = imgheight - 10; i > downY; i--)//减10 是为了防止特殊情况  这是更新trustline
                {
                    if (img[i -1][boundary_left[i]] == Black) trustline = i;
                }
                return true;
            }
            else return false;
        }
        #endregion



        void ImgProc(byte[][] image)//图像处理函数
        {
            try
            {
                //图像处理的函数在这里写
                //  findboundary(60, 49, 0, true, true, boundary_left, boundary_right, imgheight, imgheight);
                //findloseline(30, 4, 4);
                //showcolarray(49, 0, boundary_left, 2);
                //showcolarray(49, 0, boundary_right, 3);
                //showrow(0, 119, finalloseline , 5);
                //showrow(99, 119, finalloseline_right, 4);
                //showrow(0, 20, finalloseline_left, 4);

                oneforall();
                showcolarray(48, 0, boundary_left, 2);
                showcolarray(48, 0, boundary_right, 3);
                showcolarray(48, 0, middle_line, 4);
                showrow(0, imgwidth - 1, trustline, 5);
              //  showpoint(4, 5);
    
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + " " + ee.StackTrace);
            }
        }

    }
}
