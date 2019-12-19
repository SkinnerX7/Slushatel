using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Paint
{
    [Serializable]
    [XmlInclude(typeof(Karandash))]
    [XmlInclude(typeof(Paint.Pryam))]
    public class Instrument
    {
        public Instrument()
        {

        }
        private Karandash list1;
        private Karandash list2;
        private Karandash list3;
        public Karandash Add1
        {
            get
            {
                return list1;
            }
            set
            {
                list1 = value;
            }
        }
        public Karandash Add2
        {
            get
            {
                return list2;
            }
            set
            {
                list2 = value;
            }
        }
        public Karandash Add3
        {
            get
            {
                return list3;
            }
            set
            {
                list3 = value;
            }
        }
    }
}
