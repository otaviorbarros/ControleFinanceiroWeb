using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAO;

namespace ControleFinanceiroWeb.Controllers
{
    public class MovimentoController : PageBase
    {
        private void CarregarComboEmpresas()
        {
            EmpresaDAO objDao = new EmpresaDAO();
            ViewBag.lstEmpresa = objDao.ConsultarEmpresa(CodigoLogado);
        }
        private void CarregarComboCategoria()
        {
            CategoriaDAO objDao = new CategoriaDAO();
            ViewBag.lstCategoria = objDao.ConsultarCategoria(CodigoLogado);
        }
        private void CarregarComboConta()
        {
            ContaDAO objDao = new ContaDAO();
            ViewBag.lstConta = objDao.ConsultarConta(CodigoLogado);
        }


        public ActionResult Filtrar(string tipo, string dataIni, string dataFim)
        {
            ViewBag.dtIni = dataIni;
            ViewBag.dtFim = dataFim;
            if (dataIni == "" || dataFim == "")
            {
                ViewBag.Ret = 0;
                ViewBag.Msg = Mensagens.Msg.MensagemCampoObg;
            }
            else
            {
                DateTime dtInicia = Convert.ToDateTime(dataIni);
                DateTime dtFinal = Convert.ToDateTime(dataFim);
                string tipoPes = tipo;
                FiltrarMov(tipoPes, dtInicia, dtFinal);
            }
            MontarTitulo(3);
            return View("Consultar");
        }
        private void FiltrarMov(string tipo, DateTime dtini, DateTime dtfim)
        {
            MovimentoDAO objDao = new MovimentoDAO();
            ViewBag.Mov = objDao.ConsultarMovimento(CodigoLogado, dtini, dtfim, tipo);
          
        }
        private void MontarTitulo(int tipoPag)
        {
            switch (tipoPag)
            {
                case 1:
                    ViewBag.Titulo = "Cadastro de Movimento";
                    ViewBag.SubTitulo = "Cadastre aqui seus Movimentos";
                    break;
                case 2:
                    ViewBag.Titulo = "Alterar Movimento";
                    ViewBag.SubTitulo = "Altere aqui seus Movimentos";
                    break;
                case 3:
                    ViewBag.Titulo = "Consulta de Movimento";
                    ViewBag.SubTitulo = "Consulte aqui seus Movimentos";
                    break;
            }
        }
      
        public ActionResult Lancar()
        {
            if (CodigoLogado == 0)
            {
                return RedirectToAction("Login", "Usuario");
            }

            CarregarComboCategoria();
            CarregarComboConta();
            CarregarComboEmpresas();
            MontarTitulo(1);
            return View();
        }
        public ActionResult Gravar(string data, string tipo, string valor, string categoria, string empresa,
            string conta, string obs)
        {
            if (CodigoLogado == 0)
            {
                return RedirectToAction("Login", "Usuario");
            }

            if (data == "" || tipo == "" || valor == "" || categoria == "" || empresa == "" || conta == "")
            {
                ViewBag.Ret = 0;
                ViewBag.Msg = Mensagens.Msg.MensagemCampoObg;
            }else
            {
                tb_movimento objmov = new tb_movimento();
                MovimentoDAO objDao = new MovimentoDAO();
                objmov.id_usuario = CodigoLogado;
                objmov.tipo_movimento = tipo;
                objmov.valor_movimento = Convert.ToDecimal(valor);
                objmov.data_movimento = Convert.ToDateTime(data);
                objmov.obs_movimento = obs;
                objmov.id_empresa = Convert.ToInt32(empresa);
                objmov.id_categoria = Convert.ToInt32(categoria);
                objmov.id_conta = Convert.ToInt32(conta);

                try
                {
                    objDao.LancarMovimento(objmov);
                    ViewBag.Ret = 1;
                    ViewBag.Msg = Mensagens.Msg.MensagemSucesso;
                }
                catch 
                {
                    ViewBag.Ret = -1;
                    ViewBag.Msg = Mensagens.Msg.MensagemErro;
                    
                }
            }

            CarregarComboCategoria();
            CarregarComboConta();
            CarregarComboEmpresas();
            MontarTitulo(1);

            return View("Lancar");
        }
        public ActionResult Consultar()
        {
            CarregarComboEmpresas();
            CarregarComboCategoria();
            CarregarComboConta();
            MontarTitulo(3);
            return View();
        }



    }
}