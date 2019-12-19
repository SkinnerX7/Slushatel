using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Paint
{
    //[Serializable]
    public class Element1
    {
        private int t;
        private int col;
        private Point x;
        private Point y;
        private Element1 next;
        public Element1(Color color, int t, Point x, Point y)
        {
            Col = color.ToArgb();
            T = t;
            X = x;
            Y = y;
        }
        public Element1()
        {

        }
        public virtual int Col
        {
            get
            {
                return col;
            }
            set
            {
                col = value;
            }
        }
        public virtual int T
        {
            get
            {
                return t;
            }
            set
            {
                t = value;
            }
        }
        public Point X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public Point Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        public Element1 Next
        {
            get { return next; }
            set { next = value; }
        }
    }
    //[Serializable]
    public class Karandash : Instrument
    {
        public Karandash()
        {

        }
        public Element1 Head = null;
        /// <summary>
        /// Количество элементов
        /// </summary>
        public virtual int Count
        {
            get
            {
                Element1 t = Head;
                int count = 0;
                while (t != null)
                {
                    count++;
                    t = t.Next;
                }
                return count;
            }
        }
        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="pen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void Add(Color color,int v,Point x,Point y)
        {
            Element1 tmp = new Element1(color,v, x,y);
            if (Head == null)
            {
                Head = tmp;
                Head.Next = null;
            }
            else
            {
                Element1 t = Head;
                while (t.Next != null)
                    t = t.Next;
                t.Next = tmp;
            }
        }
        /// <summary>
        /// Удаление всех элементов
        /// </summary>
        public virtual void Clear()
        {
            Head = null;
        }
    }
}
