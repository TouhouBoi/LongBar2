﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sidebar.Core
{
    [XmlRoot("tile")]
    public class TileMetadata
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("developer")]
        public string Developer { get; set; }
        [XmlElement("version")]
        public string Version { get; set; }
        [XmlElement("description")]
        public string Description { get; set; }
        [XmlElement("icon")]
        public string Icon { get; set; }
        [XmlElement("link")]
        public string DownloadUrl { get; set; }
    }
}
