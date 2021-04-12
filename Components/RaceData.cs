using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DnDPlayerManager.Components
{
    class RaceData
    {


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public RaceData()
        {
            InitializeData();
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private void InitializeData()
        {
            string raceFilepath = Path.Combine( Directory.GetCurrentDirectory(), "..\\..\\Data\\") + "Races.xml";

            List<string> raceList = new List<string>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(raceFilepath);
            XmlNode root = xmlDoc.SelectSingleNode("Races");

            XmlNodeList raceNodeList = root.SelectNodes("Race");
            for (int x=0; x < raceNodeList.Count; ++x)
            {
                XmlNode currentRaceNode = raceNodeList[x];
                raceList.Add(currentRaceNode.Attributes["Name"].Value);
            }

            raceList.Sort();
            m_RaceList = raceList.ToArray();
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private string[] m_RaceList;
        public string[] RaceList
        {
            get
            {
                return m_RaceList;
            }
        }

    }
}
