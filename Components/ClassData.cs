using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DnDPlayerManager.Components
{
    struct SubclassData
    {
        public string m_ParentClassName;
        public List<string> m_Subclasses;
    }

    class ClassData
    {


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public ClassData()
        {
            InitializeData();
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void InitializeData()
        {
            string classFilepath = Path.Combine( Directory.GetCurrentDirectory(), "..\\..\\Data\\") + "Classes.xml";

            m_Subclasses = new Dictionary<string, SubclassData>();

            List<string> classList = new List<string>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(classFilepath);
            XmlNode root = xmlDoc.SelectSingleNode("Classes");

            XmlNodeList classNodeList = root.SelectNodes("Class");
            for (int x = 0; x < classNodeList.Count; ++x)
            {
                XmlNode currentClassNode = classNodeList[x];
                string className = currentClassNode.Attributes["Name"].Value;

                classList.Add(className);

                SubclassData subclassData = new SubclassData();
                subclassData.m_ParentClassName = className;
                subclassData.m_Subclasses = new List<string>();

                XmlNodeList subclassNodeList = currentClassNode.SelectNodes("Subclass");
                for (int y=0; y < subclassNodeList.Count; ++y)
                {
                    XmlNode subclassNode = subclassNodeList[y];
                    subclassData.m_Subclasses.Add(subclassNode.Attributes["Name"].Value);
                }

                subclassData.m_Subclasses.Sort();
                m_Subclasses.Add(className, subclassData);
            }

            classList.Sort();
            m_Classes = classList.ToArray();
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private string[] m_Classes;
        public string[] Classes
        {
            get
            {
                return m_Classes;
            }
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private Dictionary<string, SubclassData> m_Subclasses;
        public Dictionary<string, SubclassData> Subclasses
        {
            get
            {
                return m_Subclasses;
            }
        }

    }
}
