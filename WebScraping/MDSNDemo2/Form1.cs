using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using HtmlAgilityPack;
using System.Diagnostics;

namespace MDSNDemo2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var wc = new WebClient();

            // faz o download da página
            //
            string pagina = wc.DownloadString("https://social.msdn.microsoft.com/forums/pt-br/home?filter=alltypes%sort=fisrtpostdesc");
   
            // htmlAgility pack para poder filtrar os dados
            //necessário fazer dessa maneira pois já existe uma classe nátiva do .Net framkework com mesmo nome
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();

           
            htmlDocument.LoadHtml(pagina);

            dataGridView1.Rows.Clear();

            string id,titulo, postagem, exibicao, resposta, link = string.Empty;
    

            //laço de repetição que adiciona as linhas ao datagridview
            foreach (HtmlNode node in htmlDocument.GetElementbyId("threadList").ChildNodes)
            {
                if (node.Attributes.Count > 0)
                {   
                    //cada site é diferente, entao antes de começar a fazer o scrapping é ncessário análisar o site
                    id = node.Attributes["data-threadid"].Value; 
                    link = "https://social.msdn.microsoft.com/Forums/pt-BR/" + id;
                    titulo =  node.Descendants().First(x=>x.Id.Equals("threadTitle_" + id)).InnerText;
                    postagem = WebUtility.HtmlDecode(node.Descendants().First(x =>x.Attributes["class"] !=null && x.Attributes["class"].Value.Equals("lastpost")).InnerText.Replace("\n","").Replace("  ",""));
                    exibicao = WebUtility.HtmlDecode(node.Descendants().First(x =>x.Attributes["class"] !=null && x.Attributes["class"].Value.Equals("viewcount")).InnerText);
                    resposta = WebUtility.HtmlDecode(node.Descendants().First(x =>x.Attributes["class"] !=null && x.Attributes["class"].Value.Equals("replycount")).InnerText);

                    if (!string.IsNullOrEmpty(titulo))
                        dataGridView1.Rows.Add(titulo, postagem, exibicao, resposta, link);

                }
            }
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //quando o usuário clicar no link vai abrir uma página no navegador com o link gerado
            if (e.ColumnIndex == 4)
                Process.Start(new ProcessStartInfo(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()));
        }
    }
}
