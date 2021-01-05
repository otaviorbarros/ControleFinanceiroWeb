using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAO;

namespace ControleFinanceiroWeb.Controllers
{
    public class EmpresaController : PageBase
    {
        private void MontarTitulo(int tipoPag)
        {
            switch (tipoPag)
            {
                case 1:
                    ViewBag.Titulo = "Cadastro de Empresa";
                    ViewBag.SubTitulo = "Cadastre aqui suas Empresas";
                    break;
                case 2:
                    ViewBag.Titulo = "Alterar Empresa";
                    ViewBag.SubTitulo = "Altere aqui suas Empresas";
                    break;
                case 3:
                    ViewBag.Titulo = "Consulta de Empresa";
                    ViewBag.SubTitulo = "Consulte aqui suas Empresas";
                    break;
            }
        }
        // GET: Empresa
        public ActionResult Cadastrar()
        {
            MontarTitulo(1);
            return View();
        }
        public ActionResult Gravar(string cod, string nome, string telefone, string end, string site, string btn)
        {
            if (CodigoLogado == 0)
            {
                return RedirectToAction("Login", "Usuario");
            }

            string pagina = "";
            if (btn == "excluir")
            {
                int codEmp = Convert.ToInt32(cod);
                EmpresaDAO objDao = new EmpresaDAO();
                try
                {
                    objDao.ExcluirEmpresa(codEmp);
                    ViewBag.Ret = 1;
                    ViewBag.Msg = Mensagens.Msg.MensagemSucesso;
                }
                catch
                {

                    ViewBag.Ret = -1;
                    ViewBag.Msg = Mensagens.Msg.MensagemErroExclusao;
                }
                pagina = "Consultar";
                CarregarEmpresas();
                MontarTitulo(3);
            }
            else
            {
                if (nome.Trim() == "" || telefone.Trim() == "" || end.Trim() == "")
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
                    tb_empresa objEmpresa = new tb_empresa();
                    EmpresaDAO objDao = new EmpresaDAO();
                    objEmpresa.nome_empresa = nome;
                    objEmpresa.telefone_empresa = telefone;
                    objEmpresa.end_empresa = end;
                    objEmpresa.site_empresa = site;
                    objEmpresa.id_usuario = CodigoLogado;
                    objEmpresa.datacad_empresa = DateTime.Now;

                    MontarTitulo(2);
                    try
                    {
                        if (cod == null)
                        {
                            objDao.InserirEmpresa(objEmpresa);
                            pagina = "Cadastrar";
                            MontarTitulo(1);
                        }
                        else
                        {
                            objEmpresa.id_empresa = Convert.ToInt32(cod);
                            objDao.AlterarEmpresa(objEmpresa);
                            pagina = "Consultar";
                            MontarTitulo(3);
                            CarregarEmpresas();
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
        public ActionResult Alterar(int cod, string nome, string end, string tel, string site)
        {
            ViewBag.cod = cod;
            ViewBag.nome = nome;
            ViewBag.tel = tel;
            ViewBag.end = end;
            ViewBag.site = site;
            MontarTitulo(2);
            return View();
        }
        private void CarregarEmpresas()
        {
            EmpresaDAO objDao = new EmpresaDAO();
            List<tb_empresa> lstEmpresa = objDao.ConsultarEmpresa(CodigoLogado);
            ViewBag.LstEmpresa = lstEmpresa;
        }
        public ActionResult Consultar()
        {
            CarregarEmpresas();
            MontarTitulo(3);
            return View();
        }

    }
}