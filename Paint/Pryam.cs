using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Drawing;

    namespace Paint
    {
        public class Pryam : Karandash
        {
            public Pryam ()
            {

            }
            /// <summary>
            /// Добавление элемента
            /// </summary>
            /// <param name="pen"></param>
            /// <param name="x1"></param>
            /// <param name="y1"></param>
            /// <param name="x2"></param>
            /// <param name="y2"></param>
            public override void Add(Color color, int v, Point x, Point y)
            {
                Element1 tmp = new Element1(color, v, x, y);
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
            /// Количество элементов
            /// </summary>
            public int Count
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
            /// Удаление всех элементов
            /// </summary>
            public void Clear()
            {
                Head = null;
            }
        }
    }

}
