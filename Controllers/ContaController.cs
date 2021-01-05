using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAO;

namespace ControleFinanceiroWeb.Controllers
{
    public class ContaController : PageBase
    {
        private void MontarTitulo(int tipoPag)
        {
            switch (tipoPag)
            {
                case 1:
                    ViewBag.Titulo = "Cadastro de Conta";
                    ViewBag.SubTitulo = "Cadastre aqui suas Contas";
                    break;
                case 2:
                    ViewBag.Titulo = "Alterar Conta";
                    ViewBag.SubTitulo = "Altere aqui suas Contas";
                    break;
                case 3:
                    ViewBag.Titulo = "Consulta de Conta";
                    ViewBag.SubTitulo = "Consulte aqui suas Contas";
                    break;
            }
        }
        // GET: Conta
        public ActionResult Cadastrar()
        {
            MontarTitulo(1);
            return View();
        }
        public ActionResult Gravar(string nome, string ag, string num_conta, string saldo, string tipo, string cod, string btn)
        {
            if (CodigoLogado == 0)
            {
                return RedirectToAction("Login", "Usuario");
            }

            string pagina = "";
            if (btn == "excluir")
            {
                int codConta = Convert.ToInt32(cod);
                ContaDAO objDao = new ContaDAO();
                CarregarContas();
                try
                {
                    objDao.ExcluirConta(codConta);
                    ViewBag.Ret = 1;
                    ViewBag.Msg = Mensagens.Msg.MensagemSucesso;
                }
                catch (Exception)
                {

                    
                    ViewBag.Ret = -1;
                    ViewBag.Msg = Mensagens.Msg.MensagemErro;
                }
                pagina = "Consultar";
                CarregarContas();
                MontarTitulo(3);
            }
            else
            {
                if (nome.Trim() == "" || ag.Trim() == "")

                {
                    ViewBag.Ret = 0;
                    ViewBag.Msg = Mensagens.Msg.MensagemCampoObg;
                    if (cod == null)
                    {
                        pagina = "Cadastrar";
                        MontarTitulo(1);
                    }
                    else
                    {
                        pagina = "Alterar";
                        MontarTitulo(3);
                    }
                }
                else
                {
                    tb_conta objConta = new tb_conta();
                    ContaDAO objDao = new ContaDAO();

                    objConta.banco_conta = nome.Trim();
                    objConta.ag_conta = ag.Trim();
                    objConta.num_conta = num_conta;
                    objConta.saldo_conta = Convert.ToDecimal(saldo);
                    objConta.tipo_conta = tipo;
                    objConta.data_conta = DateTime.Now;
                    objConta.id_usuario = CodigoLogado;
                    try
                    {
                        if (cod == null)
                        {
                            objDao.InserirConta(objConta);
                            pagina = "Cadastrar";
                            MontarTitulo(1);
                        }
                        else
                        {
                            objConta.id_conta = Convert.ToInt32(cod);
                            objDao.AlterarConta(objConta);
                            pagina = "Consultar";
                            MontarTitulo(3);
                            CarregarContas();
                        }
                        ViewBag.Ret = 1;
                        ViewBag.Msg = Mensagens.Msg.MensagemSucesso;
                    }
                    catch (Exception)
                    {

                        if (cod == "")
                        {
                            pagina = "Cadastrar";
                            MontarTitulo(1);
                        }
                        else
                        {
                            pagina = "Alterar";
                            MontarTitulo(3);
                        }
                        ViewBag.Ret = -1;
                        ViewBag.Msg = Mensagens.Msg.MensagemErro;
                    }
                }
            }

            return View(pagina);

        }
        public ActionResult Alterar(string btn, string cod, string banco, string ag,
            string numero, string saldo, char tipo)
        {
            ViewBag.cod = cod;
            ViewBag.nome = banco;
            ViewBag.ag = ag;
            ViewBag.numero = numero;
            ViewBag.saldo = saldo;
            ViewBag.tipo = tipo;
            MontarTitulo(2);
            return View();
        }
        public ActionResult Consultar()
        {
            CarregarContas();
            MontarTitulo(3);
            return View();
        }

        private void CarregarContas()
        {
            ContaDAO objDao = new ContaDAO();
            List<tb_conta> lstConta = objDao.ConsultarConta(CodigoLogado);
            ViewBag.lstConta = lstConta;
        }
    }
}