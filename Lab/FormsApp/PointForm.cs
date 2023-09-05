using Newtonsoft.Json;
using PointLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;

namespace FormsApp
{
    public partial class PointForm : Form
    {
        private Point[] points = null;

        public PointForm()
        {
            InitializeComponent();
        }

        private void PointForm_Load(object sender, EventArgs e)
        {

        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Create_Click(object sender, EventArgs e)
        {
            points = new Point[5];

            var rnd = new Random();

            for (int i = 0; i < points.Length; i++)
                points[i] = rnd.Next(3) % 2 == 0 ? new Point() : new Point3D();
            listBox.DataSource = points;
        }

        private void Sort_Click(object sender, EventArgs e)
        {
            if (points == null)
                return;
            Array.Sort(points);
            listBox.DataSource = null;
            listBox.DataSource = points;
        }

        private void Serialize_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "SOAP|*.soap|XML|*.xml|JSON|*.json|Binary|*.bin";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            using (var fs =
            new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write))
            {
                switch (Path.GetExtension(dlg.FileName))
                {
                    case ".bin":
                        var bf = new BinaryFormatter();
                        bf.Serialize(fs, points);
                        break;
                    case ".soap":
                        var sf = new SoapFormatter();
                        sf.Serialize(fs, points);
                        break;
                    case ".xml":
                        var xf = new XmlSerializer(typeof(Point[]), new[]
                        {typeof(Point3D)});
                        xf.Serialize(fs, points);
                        break;
                    case ".json":
                        var jf = new DataContractJsonSerializer(typeof(Point[]), new[] { typeof(Point3D), typeof(Point) });
                        jf.WriteObject(fs, points);
                        break;
                }
            }
        }

        private void Deserialize_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "SOAP|*.soap|XML|*.xml|JSON|*.json|Binary|*.bin";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            using (var fs =
            new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
            {
                switch (Path.GetExtension(dlg.FileName))
                {
                    case ".bin":
                        var bf = new BinaryFormatter();
                        points = (Point[])bf.Deserialize(fs);
                        break;
                    case ".soap":
                        var sf = new SoapFormatter();
                        points = (Point[])sf.Deserialize(fs);
                        break;
                    case ".xml":
                        var xf = new XmlSerializer(typeof(Point[]), new[]{typeof(Point3D)});
                        points = (Point[])xf.Deserialize(fs);
                        break;
                    case ".json":
                        var jf = new DataContractJsonSerializer(typeof(Point[]), new[] { typeof(Point3D), typeof(Point) });
                        points = (Point[])jf.ReadObject(fs);
                        break;
                }
            }
            listBox.DataSource = null;
            listBox.DataSource = points;
        }
    }
}
