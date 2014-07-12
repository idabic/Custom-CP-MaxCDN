using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxCDN_CP
{
    public partial class CreateAZone : Form
    {
        public CreateAZone()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text != "") && (textBox2.Text != "") && (tabControl1.SelectedIndex == 0))
            {
                create_a_pull_zone(textBox1.Text, textBox2.Text, textBox3.Text, "");
            }
            if((textBox4.Text != "") && (textBox5.Text != "") && (tabControl1.SelectedIndex == 1))
            {
                create_a_pull_zone(textBox5.Text, "", "", textBox4.Text);
            }
            if ((textBox6.Text != "") && (textBox7.Text != "") && (tabControl1.SelectedIndex == 2))
            {
                create_a_pull_zone(textBox7.Text, "", "", textBox6.Text);
            }
        }
        private void create_a_pull_zone(string name, string url, string label, string password){
            var requestTimeout = 30;
            var alias = Properties.Settings.Default._alias;
            var consumer_key = Properties.Settings.Default._consumer_key;
            var consumer_secret = Properties.Settings.Default._consumer_secret;
            var api = new MaxCDN.Api(alias, consumer_key, consumer_secret, requestTimeout);
            if (tabControl1.SelectedIndex == 0)
            {
                api.Post("/zones/pull.json", "name=" + name + "&url=" + url + "&label=" + label);
            }
            if (tabControl1.SelectedIndex == 1)
            {
                api.Post("/zones/push.json", "name=" + name + "&password=" + password);
            }
            if (tabControl1.SelectedIndex == 2)
            {
                api.Post("/zones/vod.json", "name=" + name + "&password=" + password);
            }
        }        
    }
}
