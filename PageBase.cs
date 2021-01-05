using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleFinanceiroWeb
{
    public class PageBase : Controller
    {
        public int CodigoLogado
        {
            get
            {
                //usuario logado fixo simulado o 1 logado
                return (Session["Cod"] == null ? 0 : Convert.ToInt32(Session["Cod"]));
            }
            set
            {
                Session.Add("Cod", value);
            }
            
        }
    }
}