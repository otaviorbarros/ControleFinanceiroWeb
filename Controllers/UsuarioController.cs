using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAO;

namespace ControleFinanceiroWeb.Controllers
{
    public class UsuarioController : PageBase
    {
        // GET: Usuario
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Cadastro()
        {
            return View();
        }
        public ActionResult CriarConta(string nome, string email, string senha, string rsenha)
        {
            string pagina = "";
            ViewBag.nome = nome;
            ViewBag.email = email;
            if(nome.Trim() == "" || email.Trim() == "" || senha.Trim() == "" || rsenha.Trim() == "")
            {
                ViewBag.Msg = Mensagens.Msg.MensagemCampoObg;
                ViewBag.Ret = 0;
            }
            else if (senha.Trim() != rsenha.Trim())
            {
                ViewBag.Msg = Mensagens.Msg.MensagemSenhaNaoConfere;
                ViewBag.Ret = 0;
            }
            else
            {
                tb_usuario objUsuario = new tb_usuario();
                UsuarioDAO objDao = new UsuarioDAO();
                objUsuario.nome_usuario = nome;
                objUsuario.email_usuario = email;
                objUsuario.senha_usuario = senha;
                objUsuario.data_cadastro = DateTime.Now;

                try
                {
                    objDao.CriarConta(objUsuario);
                    ViewBag.Msg = Mensagens.Msg.MensagemSucesso;
                    ViewBag.Ret = 1;
                    ViewBag.nome = "";
                    ViewBag.email = "";
                    pagina = "Login";

                }
                catch 
                {
                    ViewBag.Msg = Mensagens.Msg.MensagemErro;
                    ViewBag.Msg = -1;
                    
                }
            }
            return View(pagina);
        }
        public ActionResult Logar(string email, string senha)
        {
            string pagina = "";
            if (email.Trim() == "" || senha.Trim() == "")
            {
                ViewBag.Msg = Mensagens.Msg.MensagemCampoObg;
                ViewBag.Ret = 0;
                pagina = "Login";
            }else
            {
                UsuarioDAO objDao = new UsuarioDAO();
                int codLogado = objDao.ValidarLogin(email, senha);

                if (codLogado == -1)
                {
                    ViewBag.Msg = Mensagens.Msg.MensagemRegistroNaoEncontrado;
                    ViewBag.Ret = 0;
                    pagina = "Login";
                }
                else
                {
                    CodigoLogado = codLogado;
                   return RedirectToAction("Consultar", "Movimento");
                }
            }
            return View(pagina);
        }
        public ActionResult Deslogar()
        {
            Session.Remove("Cod");
            return RedirectToAction("Login", "Usuario");
        }
    }
}