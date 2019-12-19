using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Drawing.Drawing2D;
using System.Threading;

namespace Paint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            g = panel.CreateGraphics();
            OnProgress += ChangeProgress;
            OnComplete += Form1_OnComplete;
        }

        private void Form1_OnComplete(bool ifComplete)
        {
            if (!ifComplete)
            {
                Message("Произошла ошибка во время копирования");
                if (File.Exists(OtkudaText))
                    File.Delete(KudaText);
            }
        }

        /// <summary>
        /// Текущий цвет
        /// </summary>
        Color Color = Color.Black;
        /// <summary>
        /// Переменная для опеределения когда можно рисовать на panel
        /// </summary>
        bool isPressed = false;
        /// <summary>
        /// Текущая точка ресунка
        /// </summary>
        Point CurrentPoint;
        /// <summary>
        /// Это начальная точка рисунка
        /// </summary>
        Point PrevPoint;  
        /// <summary>
        /// Графическая переменная
        /// </summary>
        Graphics g;
        /// <summary>
        /// Диалоговое окно для выбора цвета
        /// </summary>
        ColorDialog colorDialog = new ColorDialog();
        /// <summary>
        /// Выбор цвета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Colors_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Color = colorDialog.Color; //меняем цвет для пера
                label2.BackColor = colorDialog.Color; //меняем цвет для Фона label2
            }
        }
        /// <summary>
        /// Очистить холст
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_Click(object sender, EventArgs e)
        {
            panel.Refresh();
            list.Clear();
            list2.Clear();
            list3.Clear();
        }
        /// <summary>
        /// Нажали на холст
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            isPressed = true;
            if(flag==-1)
                CurrentPoint = e.Location;
            if (flag == 0)
                PrevPoint = e.Location;
            if (flag == 1)
                PrevPoint = e.Location;
        }
        /// <summary>
        /// Ведем курсор по холсту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPressed)//пока жмём рисуем
            {
                if (flag == -1) //перо
                {
                    PrevPoint = CurrentPoint;
                    CurrentPoint = e.Location;
                    paint();
                }
            }
        }
        /// <summary>
        /// Переменная для задавания цвета и толщины разным объектам
        /// </summary>
        Pen pen;
        /// <summary>
        /// Лист, хранящий линии карандаша
        /// </summary>
        Karandash list = new Karandash();
        /// <summary>
        /// Лист, хранящий прямоугольники
        /// </summary>
        Paint.Pryam list2 = new Paint.Pryam();
        /// <summary>
        /// Лист, хранящий эллипсы
        /// </summary>
        Paint.Pryam list3 = new Paint.Pryam();
        
        /// <summary>
        /// Метод, рисующий карандашом
        /// </summary>
        private void paint()
        {
            fl = false;
            pen = new Pen(Color, (float)numericUpDown1.Value); //Создаем перо, задаем ему цвет и толщину.
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            pen.LineJoin = LineJoin.Round;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            list.Add(pen.Color, (int)pen.Width, CurrentPoint, PrevPoint);
            g.DrawLine(pen, CurrentPoint, PrevPoint);

        }

        /// <summary>
        /// Метод, рисующий прямоугольники и эллипсы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel_MouseUp(object sender, MouseEventArgs e)
        {
            isPressed = false;
            fl = false;
            if (flag == -1) // если рисуем карандашом, уходим из метода
                return;
            CurrentPoint = e.Location;
            pen = new Pen(Color, (float)numericUpDown1.Value);
            int width = CurrentPoint.X - PrevPoint.X; // запоминание координат
            int heidht = CurrentPoint.Y - PrevPoint.Y; // запоминание координат
            if (width < 0 && heidht < 0) // Перенос ключевых точек для рисования
            {
                Point t = CurrentPoint;
                CurrentPoint = PrevPoint;
                PrevPoint = t;
            }
            else if (width > 0 && heidht < 0) // Перенос ключевых точек для рисования
            {
                Point def1 = PrevPoint;
                Point def2 = CurrentPoint;
                PrevPoint.X = def1.X;
                PrevPoint.Y = def2.Y;
                CurrentPoint.X = def2.X;
                CurrentPoint.Y = def1.Y;

            }
            else if (width < 0 && heidht > 0) // Перенос ключевых точек для рисования
            {
                Point def1 = PrevPoint;
                Point def2 = CurrentPoint;
                PrevPoint.X = def2.X;
                PrevPoint.Y = def1.Y;
                CurrentPoint.X = def1.X;
                CurrentPoint.Y = def2.Y;
            }
            if (flag == 0) // рисуем прямоугольник
            {
                g.DrawRectangle(pen, PrevPoint.X, PrevPoint.Y, CurrentPoint.X - PrevPoint.X, CurrentPoint.Y - PrevPoint.Y);
                list2.Add(pen.Color, (int)pen.Width, PrevPoint,CurrentPoint);
            }
            if(flag == 1) // рисуем эллипс
            {
                g.DrawEllipse(pen, PrevPoint.X, PrevPoint.Y, CurrentPoint.X - PrevPoint.X, CurrentPoint.Y - PrevPoint.Y);
                list3.Add(pen.Color,(int)pen.Width, PrevPoint, CurrentPoint);
            }
        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Выйти ?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Версия 10.1705.21204.0 \n © Корпорация Весёлый Чув(Veselyi Tchuv Corporation), 2019.\n Все права защищены.\n", "Справка", MessageBoxButtons.OK);

        }

        /// <summary>
        /// Метод, запускающийся при изменении размеров формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Ref();
        }
        /// <summary>
        /// Флаг для определения когда отрисовывать листы, а когда файл из загрузки
        /// </summary>
        bool fl = false;
        /// <summary>
        /// Перерисовка объектов
        /// </summary>
        private void Ref()
        {
            Pen p;
            if (fl)
            {
                list = inst.Add1;
                list2 = (Paint.Pryam)inst.Add2;
                list3 = (Paint.Pryam)inst.Add3;
            }
            if (list.Count!=0)
            {
                Element1 t = list.Head;
                for (int i = 0; i < list.Count; i++)
                {
                    Color col = Color.FromArgb(t.Col);
                    p = new Pen(col, t.T);
                    p.StartCap = LineCap.Round;
                    p.EndCap = LineCap.Round;
                    p.LineJoin = LineJoin.Round;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawLine(p,t.X, t.Y);
                    t = t.Next;
                }
            }
            if (list2.Count != 0)
            {
                Element1 t = list2.Head;
                for (int i = 0; i < list2.Count; i++)
                {
                    Color col = Color.FromArgb(t.Col);
                    p = new Pen(col, t.T);
                    g.DrawRectangle(p, t.X.X,t.X.Y,t.Y.X- t.X.X, t.Y.Y- t.X.Y);
                    t = t.Next;
                }
            }
            if (list3.Count != 0)
            {
                Element1 t = list3.Head;
                for (int i = 0; i < list3.Count; i++)
                {
                    Color col = Color.FromArgb(t.Col);
                    p = new Pen(col, t.T);
                    g.DrawEllipse(p, t.X.X, t.X.Y, t.Y.X - t.X.X, t.Y.Y - t.X.Y);
                    t = t.Next;
                }
            }

        }
        /// <summary>
        /// Флаг для определения, чем сейчас рисуют (-1 - "Карандаш",0 - "Прямоугольник", 1 - "Эллипс")
        /// </summary>
        private int flag = -1;
        /// <summary>
        /// Выбрали карандаш
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pero_Click(object sender, EventArgs e)
        {
            flag = -1;
        }
        /// <summary>
        /// Выбрали прямоугольник
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kvadrat_Click(object sender, EventArgs e)
        {
            flag = 0;
        }

        /// <summary>
        /// Выбрали эллипс
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void elips_Click(object sender, EventArgs e)
        {
            flag = 1;
        }
        XmlSerializer formatter = new XmlSerializer(typeof(Instrument));
        Instrument inst = new Instrument();
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                inst.Add1= list;
                inst.Add2 = list2;
                inst.Add3 = list3;
                string name = saveFileDialog1.FileName;
                if (name.IndexOf('.') != -1)
                {
                    name = name.Remove(name.IndexOf('.'), 4);
                }
                using (FileStream fs = new FileStream(name + ".xml", FileMode.OpenOrCreate))
                {
                    statusStrip1.Show();
                    formatter.Serialize(fs, inst);
                }
            }
        }

        private void открытьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string name = openFileDialog1.FileName;
                    string xml = name.Remove(0, name.Length - 3);
                    if (xml != "xml")
                        throw new Exception("Неверный формат");
                    using (FileStream fs = new FileStream(name, FileMode.Open))
                    {
                        list.Clear();
                        list2.Clear();
                        list3.Clear();
                        panel.Refresh();
                        inst = (Instrument)formatter.Deserialize(fs);
                        fl = true;
                        Ref();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                otkuda.Text = ofd.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (otkuda.Text == "...")
                Message("Выберите что копировать");
            else
            {
                if (!File.Exists(otkuda.Text))
                    Message("Выбранный файл был удален!");
                else
                {
                    int index = otkuda.Text.LastIndexOf('.');
                    string format = otkuda.Text.Remove(0, index);
                    SaveFileDialog ofd = new SaveFileDialog() { Filter = $"{format}(*{format})|*{format}" };
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        kuda.Text = ofd.FileName;
                    }
                }
            }
        }
        /// <summary>
        /// Путь к исходному файлу
        /// </summary>
        public static string OtkudaText;
        /// <summary>
        /// Путь для копирования
        /// </summary>
        public static string KudaText;
        /// <summary>
        /// Сообщение
        /// </summary>
        private static string msg;

        /// <summary>
        /// Поток, занимающийся копированием файла
        /// </summary>
        Thread thread = new Thread(new ThreadStart(Copy));

        public delegate void Complet(bool ifComplete);
        public delegate void Progress(string message, int procent);

        /// <summary>
        /// Если скопировалась часть файла, отрабатывает это событие
        /// </summary>
        public static event Progress OnProgress;
        /// <summary>
        /// Событие, которое равно true - если копирование успешно завершилось, иначе false
        /// </summary>
        public static event Complet OnComplete;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            int index = otkuda.Text.LastIndexOf('.');
            string format = otkuda.Text.Remove(0, index);
            index = kuda.Text.LastIndexOf('.');
            string format2 = kuda.Text.Remove(0, index);
            if (format != format2)
                Message("Форматы файлов не совпадают");
            else
            {
                if (!File.Exists(otkuda.Text))
                    Message("Выбранный файл не найден");
                else
                {
                    if (thread.ThreadState == ThreadState.Suspended)
                    {
                        thread.Resume();
                    }
                    else if (thread.ThreadState == ThreadState.Unstarted)
                    {
                        OtkudaText = otkuda.Text;
                        KudaText = kuda.Text;
                        thread.Start();
                    }
                    else if (thread.ThreadState == ThreadState.Aborted || thread.ThreadState == ThreadState.Stopped)
                    {
                        thread = new Thread(new ThreadStart(Copy));
                        OtkudaText = otkuda.Text;
                        KudaText = kuda.Text;
                        thread.Start();
                    }
                }
            }
        }

        /// <summary>
        /// Копирование файла
        /// </summary>
        private static void Copy()
        {
            int BufferLenght = 1024;
            try
            {
                Byte[] streamBuffer = new Byte[BufferLenght];
                long totalBytesRead = 0;
                int numReads = 0;

                using (FileStream sourceStream = new FileStream(OtkudaText, FileMode.Open, FileAccess.Read))
                {
                    long sLenght = sourceStream.Length;
                    using (FileStream destinationStream = new FileStream(KudaText, FileMode.Create, FileAccess.Write))
                    {
                        while (true)
                        {
                            Thread.Sleep(50);
                            numReads++;
                            int bytesRead = sourceStream.Read(streamBuffer, 0, BufferLenght);

                            if (bytesRead == 0)
                            {
                                getInfo(sLenght, sLenght);
                                break;
                            }

                            destinationStream.Write(streamBuffer, 0, bytesRead);
                            totalBytesRead += bytesRead;

                            if (numReads % 10 == 0)
                            {
                                getInfo(totalBytesRead, sLenght);
                            }

                            if (bytesRead < BufferLenght)
                            {
                                getInfo(totalBytesRead, sLenght);
                                break;
                            }
                        }
                    }
                }

                OnComplete?.Invoke(true);
            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception e)
            {
                MessageBox.Show("Возникла следующая ошибка при копировании:\n" + e.Message);
                OnComplete?.Invoke(false);
            }
        }

        /// <summary>
        /// Формирование строки текущего состояния
        /// </summary>
        /// <param name="totalBytesRead"></param>
        /// <param name="sLenght"></param>
        private static void getInfo(long totalBytesRead, long sLenght)
        {
            string message = string.Empty;
            double pctDone = (double)((double)totalBytesRead / (double)sLenght);
            message = string.Format("Считано: {0} из {1}. Всего {2}%",totalBytesRead, sLenght, (int)(pctDone * 100));
            OnProgress?.Invoke(message, (int)(pctDone * 100));
        }

        /// <summary>
        /// Обновление прогресса
        /// </summary>
        /// <param name="message"></param>
        /// <param name="procent"></param>
        private void ChangeProgress(string message, int procent)
        {
            if (procent == 100)
            {
                progressBar1.Invoke((MethodInvoker)delegate { progressBar1.Value = 0; });
                Message(message);
            }
            else
            {
            progressBar1.Invoke((MethodInvoker)delegate { progressBar1.Value = procent; });
            Message(message);
            }

        }

        /// <summary>
        /// Контроль за доступом из всех потоков к таблу
        /// </summary>
        /// <param name="Message"></param>
        private void Message(string Message)
        {
            msg = Message;
            tablo.Invoke(new MethodInvoker(mes));
        }

        private void mes()
        {
            tablo.Text = msg;
        }

        /// <summary>
        /// Пауза
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (thread.ThreadState == ThreadState.WaitSleepJoin)
            {
                thread.Suspend();
                Message("На паузе");
            }
        }

        /// <summary>
        /// Стоп
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            if (thread.ThreadState != ThreadState.Unstarted && thread.ThreadState != ThreadState.Aborted)
            {
                if (thread.ThreadState == ThreadState.Suspended)
                    thread.Resume();
                thread.Abort();
                progressBar1.Value = 0;
                Message("Операция была отменена!");
                if (thread.ThreadState != ThreadState.Stopped)
                    if (File.Exists(KudaText))
                        File.Delete(KudaText);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(progressBar1.Value>0&& progressBar1.Value < 100)
            {
                MessageBox.Show("Невозможно закрыть форму: идёт копирование файла");
                e.Cancel = true;
            }
        }
    }
}
