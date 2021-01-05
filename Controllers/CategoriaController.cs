using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAO;
namespace ControleFinanceiroWeb.Controllers
{
    public class CategoriaController : PageBase
    {
        private void MontarTitulo(int tipoPag)
        {
            switch (tipoPag)
            {
                case 1:
                    ViewBag.Titulo = "Cadastro de Categoria";
                    ViewBag.SubTitulo = "Cadastre aqui suas Categorias";
                    break;
                case 2:
                    ViewBag.Titulo = "Alterar de Categoria";
                    ViewBag.SubTitulo = "Altere aqui suas Categorias";
                    break;
                case 3:
                    ViewBag.Titulo = "Consulta de Categoria";
                    ViewBag.SubTitulo = "Consulte aqui suas Categorias";
                    break;
            }
        }
        private void CarregarCategorias()
        {
            CategoriaDAO objDao = new CategoriaDAO();
            List<tb_categoria> lst = objDao.ConsultarCategoria(CodigoLogado);
            ViewBag.LstCategoria = lst;
        }
        // GET: Categoria
        public ActionResult Cadastrar()
        {
            MontarTitulo(1);
            return View();
        }
        public ActionResult Gravar(string cod, string nome, string btn)
        {
            if (CodigoLogado == 0)
            {
                return RedirectToAction("Login", "Usuario");
            }

            string pagina = "";
            if (btn == "excluir")
            {
                int codCat = Convert.ToInt32(cod);
                CategoriaDAO objDao = new CategoriaDAO();
                CarregarCategorias();
                try
                {

                    objDao.ExcluirCategoria(codCat);
                    ViewBag.Ret = 1;
                    ViewBag.Msg = Mensagens.Msg.MensagemSucesso;
                }
                catch
                {
                    ViewBag.Ret = -1;
                    ViewBag.Msg = Mensagens.Msg.MensagemErroExclusao;
                }
                pagina = "Consultar";
                CarregarCategorias();
                MontarTitulo(3);
            }
            else
            {
                if (nome.Trim() == "")
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
                    tb_categoria objCategoria = new tb_categoria();
                    CategoriaDAO objDao = new CategoriaDAO();
                    objCategoria.nome_categoria = nome;
                    objCategoria.id_usuario = CodigoLogado;
                  

                    try
                    {
                        if (cod == null)
                        {
                            objCategoria.data_categoria = DateTime.Now;
                            objDao.InserirCategoria(objCategoria);
                            pagina = "Cadastrar";
                            MontarTitulo(1);
                        }
                        else
                        {
                            objCategoria.id_categoria = Convert.ToInt32(cod);
                            objDao.AlterarCategoria(objCategoria);
                            pagina = "Consultar";
                            MontarTitulo(3);
                            CarregarCategorias();
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
        public ActionResult Consultar()
        {
            CarregarCategorias();
            MontarTitulo(3);
            return View();
        }
        public ActionResult Alterar(int cod, string nome)
        {
            ViewBag.cod = cod;
            ViewBag.nome = nome;
            MontarTitulo(2);
            return View();
        }
    }

}