using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MaxCDN;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MaxCDN_CP
{
    public partial class Form1 : Form
    {
        string ztype = "";
        public Form1()
        {
            InitializeComponent();
            reports_summ();
            acc();
        }

        private void acc()
        {
            panel1.Visible = true;
            label1.Visible = true;
            label3.Visible = true;
            var requestTimeout = 30;
            var alias = Properties.Settings.Default._alias;
            var consumer_key = Properties.Settings.Default._consumer_key;
            var consumer_secret = Properties.Settings.Default._consumer_secret;
            var api = new MaxCDN.Api(alias, consumer_key, consumer_secret, requestTimeout);
            var res = api.Get("/account.json");
            string result = Convert.ToString(JObject.Parse(res));
            dynamic jsondes = JsonConvert.DeserializeObject(result);
            var name = jsondes.data.account.name;
            label2.Text = name;
            label4.Text = jsondes.data.account.date_created;
        }

        private void pULLToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            ztype = "pull";
            var requestTimeout = 30;
            var alias = Properties.Settings.Default._alias;
            var consumer_key = Properties.Settings.Default._consumer_key;
            var consumer_secret = Properties.Settings.Default._consumer_secret;
            var api = new MaxCDN.Api(alias, consumer_key, consumer_secret, requestTimeout);
            var res = api.Get("/zones/pull.json");
            string result = Convert.ToString(JObject.Parse(res));
            dynamic jsondes = JsonConvert.DeserializeObject(result);
            JArray names = jsondes.data.pullzones;
            comboBox1.Items.Add("-- Select a Zone --");
            comboBox1.SelectedIndex = 0;
            foreach (object name in names)
            {
                string nameobj = Convert.ToString(JObject.Parse(Convert.ToString(name)));
                dynamic nm = JsonConvert.DeserializeObject(nameobj);
                string nmobj = nm.name;
                comboBox1.Items.Add(Convert.ToString(nmobj));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            var requestTimeout = 30;
            var alias = Properties.Settings.Default._alias;
            var consumer_key = Properties.Settings.Default._consumer_key;
            var consumer_secret = Properties.Settings.Default._consumer_secret;
            var api = new MaxCDN.Api(alias, consumer_key, consumer_secret, requestTimeout);
            var res = api.Get("/zones/" + ztype + ".json/" + comboBox1.SelectedItem.ToString());
            string result = Convert.ToString(JObject.Parse(res));
            dynamic jsondes = JsonConvert.DeserializeObject(result);
            string gzip = jsondes.data.pullzone.compress;
            string qs = jsondes.data.pullzone.queries;
            string use_stale = jsondes.data.pullzone.use_stale;
            string strip_cookies = jsondes.data.pullzone.ignore_setcookie_header;
            if (gzip == "1") {checkBox1.Checked = true;}
            if (qs == "1") { checkBox2.Checked = true; }
            if (use_stale == "1") { checkBox3.Checked = true; }
            if (strip_cookies == "1") { checkBox4.Checked = true; }
            textBox1.Text = jsondes.data.pullzone.url;
            textBox2.Text = jsondes.data.pullzone.label;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((comboBox1.SelectedItem.ToString() != "") || (comboBox1.SelectedItem.ToString() != "-- Select a Zone --"))
            {
                update_pull_settings();
            }
        }
        private void update_pull_settings()
        {
            string gzip = "0"; string queries = "0"; string use_stale = "0"; string ignore_setcookie_header = "0";
            if (checkBox1.Checked) { gzip = "1"; }
            if (checkBox2.Checked) { queries = "1"; }
            if (checkBox3.Checked) { use_stale = "1"; }
            if (checkBox4.Checked) { ignore_setcookie_header = "1"; }
            var requestTimeout = 30;
            var alias = Properties.Settings.Default._alias;
            var consumer_key = Properties.Settings.Default._consumer_key;
            var consumer_secret = Properties.Settings.Default._consumer_secret;
            var api = new MaxCDN.Api(alias, consumer_key, consumer_secret, requestTimeout);
            api.Put("/zones/" + ztype + ".json/" + comboBox1.SelectedItem.ToString(), "compress=" + gzip + "&queries=" + queries + "&ignore_setcookie_header=" + ignore_setcookie_header + "&use_stale=" + use_stale);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                update_pull_overview();
            }
        }
        private void update_pull_overview(){
            var requestTimeout = 30;
            var alias = Properties.Settings.Default._alias;
            var consumer_key = Properties.Settings.Default._consumer_key;
            var consumer_secret = Properties.Settings.Default._consumer_secret;
            var api = new MaxCDN.Api(alias, consumer_key, consumer_secret, requestTimeout);
            api.Put("/zones/" + ztype + ".json/" + comboBox1.SelectedItem.ToString(), "url=" + textBox1.Text + "&label=" + textBox2.Text);
    }

        private void button4_Click(object sender, EventArgs e)
        {
            if ((comboBox1.SelectedItem.ToString() != "") || (comboBox1.SelectedItem.ToString() != "-- Select a Zone --"))
            {
                delete_zone();
            }
        }
        private void delete_zone()
        {
            var requestTimeout = 30;
            var alias = Properties.Settings.Default._alias;
            var consumer_key = Properties.Settings.Default._consumer_key;
            var consumer_secret = Properties.Settings.Default._consumer_secret;
            var api = new MaxCDN.Api(alias, consumer_key, consumer_secret, requestTimeout);
            api.Delete("/zones/" + ztype + ".json/" + comboBox1.SelectedItem.ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CreateAZone createpull = new CreateAZone();
            createpull.Show();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            
        }
        private void reports_summ()
        {
            var requestTimeout = 30;
            var alias = Properties.Settings.Default._alias;
            var consumer_key = Properties.Settings.Default._consumer_key;
            var consumer_secret = Properties.Settings.Default._consumer_secret;
            var api = new MaxCDN.Api(alias, consumer_key, consumer_secret, requestTimeout);
            var res = api.Get(("/reports/statsbyzone.json/monthly"));
            string result = Convert.ToString(JObject.Parse(res));
            dynamic jsondes = JsonConvert.DeserializeObject(result);
            string requests = jsondes.data.summary.hit;
            string hits = jsondes.data.summary.cache_hit;
            string misses = jsondes.data.summary.noncache_hit;

            string[] seriesArray = { "Requests", "Hits", "Misses" };
            int[] pointsArray = {Convert.ToInt32(requests), Convert.ToInt32(hits), Convert.ToInt32(misses) };

            // Set palette.
            this.chart2.Palette = ChartColorPalette.SeaGreen;

            // Set title.
            this.chart2.Titles.Add("Summary");

            // Add series.
            for (int i = 0; i < seriesArray.Length; i++)
            {
                // Add series.
                Series series = this.chart2.Series.Add(seriesArray[i]);

                // Add point.
                series.Points.Add(pointsArray[i]);
            }
        }
    }
}
