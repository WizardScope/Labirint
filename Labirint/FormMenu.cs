using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Labirint
{
    public partial class FormMenu : Form
    {
        const int N = 21;
        const int M = 21;
        static int[,] m = new int[N, M];
        int x1 = 1;
        int y1 = 0;
        int x2 = 0;
        int y2 = -1;

        bool flag1 = false;

        public FormMenu()
        {
            InitializeComponent();
            //SetSize();
        }
        //Bitmap map = new Bitmap(100, 100);

        //Graphics graphics;

        //private void SetSize()
        //{
        //    Rectangle rectangle = Screen.PrimaryScreen.Bounds;
        //    map = new Bitmap(rectangle.Width, rectangle.Height);
        //    graphics = Graphics.FromImage(map);
        //}

        public void Labirint_Generate()
        {
            // Заполняю массив стенами
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    m[i, j] = 1;
                }
            }

            // Создаю точку входа и выхода в лабиринте
            m[0, 5] = 0;
            m[20, 5] = 0;

            // Создаю генератор случайных чисел
            Random random = new Random();

            // Создаю список для хранения текущих ячеек лабиринта
            List<int[]> cells = new List<int[]>();

            // Добавляю начальную ячейку в список
            cells.Add(new int[] { 1, 5 });

            // Пока список не пустой, происходит генерация лабиринта
            while (cells.Count != 0)
            {
                // Выбирается случайная ячейка из списка
                int index = random.Next(cells.Count);
                int x = cells[index][0];
                int y = cells[index][1];

                // Список для хранения соседних ячеек
                List<int[]> neighbors = new List<int[]>();

                // Проверка соседних ячейеек по горизонтали и вертикали
                // Если ячейка находится в пределах массива и является стеной,
                // то добавляем ее в список соседей
                if (x - 2 >= 0 && m[x - 2, y] == 1)
                {
                    neighbors.Add(new int[] { x - 2, y });
                }
                if (x + 2 < 21 && m[x + 2, y] == 1)
                {
                    neighbors.Add(new int[] { x + 2, y });
                }
                if (y - 2 >= 0 && m[x, y - 2] == 1)
                {
                    neighbors.Add(new int[] { x, y - 2 });
                }
                if (y + 2 < 21 && m[x, y + 2] == 1)
                {
                    neighbors.Add(new int[] { x, y + 2 });
                }

                // Если есть соседние ячейки, то случайно выбирается одна из них
                if (neighbors.Count > 0)
                {
                    int n = random.Next(neighbors.Count);
                    int nx = neighbors[n][0];
                    int ny = neighbors[n][1];

                    // Соединяется текущая и соседняя ячейка,
                    // убирается стена между ними
                    m[(x + nx) / 2, (y + ny) / 2] = 0;
                    m[nx, ny] = 0;

                    // Выбранная соседняя ячейка добавляется в список текущих ячеек
                    cells.Add(new int[] { nx, ny });
                }
                else
                {
                    // Если нет соседних ячеек, то текущая ячейка удаляется из списка
                    cells.RemoveAt(index);
                }
            }
            m[0, 5] = 2; // на входе будет колобок )
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            Labirint_Generate();

        }
        private void Draw()
        {
            Graphics gr = this.CreateGraphics();
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(0); j++)
                {
                    
                    int size = 15; // размер будущей фигуры
                    int x = size * i;
                    int y = size * j;
                    SolidBrush myBrush = new SolidBrush(Color.Red);

                    if (m[i, j] == 0 || m[i, j] == 2)
                    {
                        myBrush = new SolidBrush(Color.Red);
                    }

                    else if (m[i,j] == 1)
                    {
                        myBrush = new SolidBrush(Color.Black);
                    }
                    gr.FillRectangle(myBrush, x, y, size, size);
                }
            }


            Graphics gr2 = this.CreateGraphics(); // сам колобок
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(0); j++)
                {
                    int size = 15;
                    int x = size * i;
                    int y = size * j;
                    if (m[i, j] == 2)
                    {
                        SolidBrush myBrush = new SolidBrush(Color.Purple);
                        gr2.FillEllipse(myBrush, x, y, size, size);
                    }

                }
            }
        }
        private void Start()
        {
            if ((m[20, 5] != 2) == true)
            {
                button3.Enabled = false;
                button1.Enabled = false;
                button5.Enabled = false;
            }

            Task.Run(() =>
            {
                Graphics gr3 = this.CreateGraphics();
                bool flag = true;
                while (m[20, 5] != 2)
                {
                    if (flag1 == true)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            button3.Enabled = true;
                            button1.Enabled = true;
                            button5.Enabled = true;
                        });
                        x1 = 1;
                        y1 = 0;
                        x2 = 0;
                        y2 = -1;
                        flag1 = false;
                        return;
                    }
                    flag = true;
                    for (int i = 0; i < m.GetLength(0); i++)
                    {
                        for (int j = 0; j < m.GetLength(0); j++)
                        {
                            if (m[i, j] == 2)
                            {
                                int size = 15;
                                int x = size * i;
                                int y = size * j;
                                SolidBrush myBrush = new SolidBrush(Color.Purple);
                                if (m[i + x1, j + y1] == 0) // шаг вправо
                                {

                                    m[i + x1, j + y1] = 2;
                                    m[i, j] = 0;
                                    if (m[0, 5] == 0)
                                    {
                                        m[0, 5] = 1;
                                    }

                                    gr3.FillEllipse(myBrush, x + size * x1, y + size * y1, size, size);
                                    myBrush = new SolidBrush(Color.Red);
                                    gr3.FillRectangle(myBrush, x, y, size, size);
                                    if (x1 == 1) // если робот пошёл вправо (вправо)
                                    {
                                        x1 = 0;
                                        y1 = 1;
                                        x2 = 1;
                                        y2 = 0;
                                    }
                                    else if (y1 == 1) // если робот пошёл вправо (вниз)
                                    {
                                        x1 = -1;
                                        y1 = 0;
                                        x2 = 0;
                                        y2 = 1; // если нет помехи - иду вниз
                                    }
                                    else if (y1 == -1)
                                    {
                                        x1 = 1;
                                        y1 = 0;
                                        x2 = 0;
                                        y2 = -1;
                                    }
                                    else if (x1 == -1)
                                    {
                                        x1 = 0;
                                        y1 = -1;
                                        x2 = -1;
                                        y2 = 0;
                                    }

                                    Thread.Sleep(70);
                                    flag = false;
                                    break;
                                }
                                else if (m[i + x2, j + y2] == 0)
                                {
                                    m[i + x2, j + y2] = 2;
                                    m[i, j] = 0;

                                    gr3.FillEllipse(myBrush, x + size * x2, y + size * y2, size, size);
                                    myBrush = new SolidBrush(Color.Red);
                                    gr3.FillRectangle(myBrush, x, y, size, size);

                                    Thread.Sleep(100);
                                    flag = false;
                                    break;
                                }
                                else
                                {
                                    if (x1 == -1)
                                    {
                                        x1 = 0;
                                        y1 = 1;
                                        x2 = 1;
                                        y2 = 0;
                                    }
                                    else if (y1 == 1)
                                    {
                                        x1 = 1;
                                        y1 = 0;
                                        x2 = 0;
                                        y2 = -1;
                                    }
                                    else if (x1 == 1)
                                    {
                                        x1 = 0;
                                        y1 = -1;
                                        x2 = -1;
                                        y2 = 0;
                                    }
                                    else if (y1 == -1)
                                    {
                                        x1 = -1;
                                        y1 = 0;
                                        x2 = 0;
                                        y2 = 1;
                                    }
                                    flag = false;
                                }
                                if (flag == false)
                                    break;
                            }
                            if (flag == false)
                                break;
                        }

                        if (flag == false)
                            break;
                    }
                }
                Invoke((MethodInvoker)delegate
                {
                    button3.Enabled = true;
                    button1.Enabled = true;
                    button5.Enabled = true;
                });
            });
        }
        private void Return()
        {
            if (button3.Enabled == false)
            {
                flag1 = true;
            }
            
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(0); j++)
                {
                    if (m[i, j] == 2)
                    {
                        m[i, j] = 0;
                    }
                }
            }
            if (m[0, 5] == 1 || m[0, 5] == 0)
                m[0, 5] = 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Draw();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Return();
            Draw();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Labirint_Generate();
            Draw();
        }
    }
}
