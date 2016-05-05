using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kinnemed05.Models
{
    public class SetPrueba
    {

        public IEnumerable<prueba> prueba { get; set; }
        
        public SelectList valores(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "", Value = "" });
            list_valores.Add(new SelectListItem { Text = "POSITIVO", Value = "POSITIVO" });
            list_valores.Add(new SelectListItem { Text = "NEGATIVO", Value = "NEGATIVO" });
            SelectList valores;
            if (String.IsNullOrEmpty(defaul))
                valores = new SelectList(list_valores, "Value", "Text");
            else
                valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;
        }
        public SelectList val_inm(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "REACTIVO", Value = "REACTIVO" });
            list_valores.Add(new SelectListItem { Text = "NO REACTIVO", Value = "NO REACTIVO" });
            SelectList valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;

        }
        public SelectList val_cul(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "", Value = "" });
            list_valores.Add(new SelectListItem { Text = "SENSIBLE", Value = "SENSIBLE" });
            list_valores.Add(new SelectListItem { Text = "INTERMEDIO", Value = "INTERMEDIO" });
            list_valores.Add(new SelectListItem { Text = "RESISTENTE", Value = "RESISTENTE" });
            SelectList valores;
            if (String.IsNullOrEmpty(defaul))
                valores = new SelectList(list_valores, "Value", "Text");
            else
                valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;

        }
        public SelectList col_cop(string defaul) {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "CAFE", Value = "CAFE" });
            list_valores.Add(new SelectListItem { Text = "AMARILLO", Value = "AMARILLO" });
            list_valores.Add(new SelectListItem { Text = "VERDOSO", Value = "VERDOSO" });
            list_valores.Add(new SelectListItem { Text = "ROJIZO", Value = "ROJIZO" });
            list_valores.Add(new SelectListItem { Text = "NEGRUZCO", Value = "NEGRUZCO" });
            SelectList valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;

        }
        public SelectList asp_cop(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "BLANDA", Value = "BLANDA" });
            list_valores.Add(new SelectListItem { Text = "PASTOSA", Value = "PASTOSA" });
            list_valores.Add(new SelectListItem { Text = "DURA", Value = "DURA" });
            list_valores.Add(new SelectListItem { Text = "LIQUIDA", Value = "LIQUIDA" });
            list_valores.Add(new SelectListItem { Text = "SEMILIQUIDA", Value = "SEMILIQUIDA" });
            SelectList valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;
        }
        public SelectList con_cop(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "HETEROGENEA", Value = "HETEROGENEA" });
            list_valores.Add(new SelectListItem { Text = "HOMOGENEA", Value = "HOMOGENEA" });
            SelectList valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;
        }

        public SelectList flo_cop(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "NORMAL", Value = "NORMAL" });
            list_valores.Add(new SelectListItem { Text = "LIGERAMENTE AUMENTADA", Value = "LIGERAMENTE AUMENTADA" });
            list_valores.Add(new SelectListItem { Text = "LIGERAMENTE DISMINUIDA", Value = "DULIGERAMENTE DISMINUIDARA" });
            list_valores.Add(new SelectListItem { Text = "AUMENTADA", Value = "AUMENTADA" });
            list_valores.Add(new SelectListItem { Text = "DISMINUIDA", Value = "DISMINUIDA" });
            SelectList valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;
        }

        public SelectList val_esc(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "ESCASAS ", Value = "ESCASAS" });
            list_valores.Add(new SelectListItem { Text = "(+)", Value = "(+)" });
            list_valores.Add(new SelectListItem { Text = "(++)", Value = "(++)" });
            list_valores.Add(new SelectListItem { Text = "(+++)", Value = "(+++)" });
            SelectList valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;
        }
        public SelectList val_neg(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "NEGATIVO ", Value = "NEGATIVO" });
            list_valores.Add(new SelectListItem { Text = "(+)", Value = "(+)" });
            list_valores.Add(new SelectListItem { Text = "(++)", Value = "(++)" });
            list_valores.Add(new SelectListItem { Text = "(+++)", Value = "(+++)" });
            SelectList valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;
        }
        public SelectList val_forma(string defaul) {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "", Value = "" });
            list_valores.Add(new SelectListItem { Text = "QUISTE", Value = "QUISTE" });
            list_valores.Add(new SelectListItem { Text = "TROFOZOITO", Value = "TROFOZOITO" });
            SelectList valores;
            if (String.IsNullOrEmpty(defaul))
                valores = new SelectList(list_valores, "Value", "Text");
            else
                valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;
        }
        public SelectList val_cantidad(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "", Value = "" });
            list_valores.Add(new SelectListItem { Text = "(+)", Value = "(+)" });
            list_valores.Add(new SelectListItem { Text = "(++)", Value = "(++)" });
            list_valores.Add(new SelectListItem { Text = "(+++)", Value = "(+++)" });
            SelectList valores;
            if (String.IsNullOrEmpty(defaul))
                valores = new SelectList(list_valores, "Value", "Text");
            else
                valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;
        }

        public SelectList col_emo(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "AMARILLO", Value = "AMARILLO" });
            list_valores.Add(new SelectListItem { Text = "ROJO", Value = "ROJO" });
            list_valores.Add(new SelectListItem { Text = "AMBAR", Value = "AMBAR" });
            list_valores.Add(new SelectListItem { Text = "PAJIZO", Value = "PAJIZO" });
            SelectList valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;

        }
        public SelectList asp_emo(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "TRANSPARENTE", Value = "TRANSPARENTE" });
            list_valores.Add(new SelectListItem { Text = "LIGERAMENTE TURBIO", Value = "LIGERAMENTE TURBIO" });
            SelectList valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;
        }

        public SelectList den_emo(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "1,000", Value = "1,000" });
            list_valores.Add(new SelectListItem { Text = "1,005", Value = "1,005" });
            list_valores.Add(new SelectListItem { Text = "1,010", Value = "1,010" });
            list_valores.Add(new SelectListItem { Text = "1,015", Value = "1,015" });
            list_valores.Add(new SelectListItem { Text = "1,020", Value = "1,020" });
            list_valores.Add(new SelectListItem { Text = "1,025", Value = "1,025" });
            list_valores.Add(new SelectListItem { Text = "1,030", Value = "1,030" });
            SelectList valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;

        }
        public SelectList ph_emo(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "5", Value = "5" });
            list_valores.Add(new SelectListItem { Text = "5,5", Value = "5,5" });
            list_valores.Add(new SelectListItem { Text = "6", Value = "6" });
            list_valores.Add(new SelectListItem { Text = "6,5", Value = "6,5" });
            list_valores.Add(new SelectListItem { Text = "7", Value = "7" });
            list_valores.Add(new SelectListItem { Text = "8", Value = "8" });
            list_valores.Add(new SelectListItem { Text = "9", Value = "9" });
            SelectList valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;

        }
        public SelectList cri_emo(string defaul)
        {
            List<SelectListItem> list_valores = new List<SelectListItem>();
            list_valores.Add(new SelectListItem { Text = "NEGATIVO", Value = "NEGATIVO" });
            list_valores.Add(new SelectListItem { Text = "URATOS AMORFOS", Value = "URATOS AMORFOS" });
            list_valores.Add(new SelectListItem { Text = "OXALATOS DE CALCIO", Value = "OXALATOS DE CALCIO" });
            list_valores.Add(new SelectListItem { Text = "ACIDO URICO", Value = "ACIDO URICO" });
            list_valores.Add(new SelectListItem { Text = "FOSFATOS TRIPLES", Value = "FOSFATOS TRIPLES" });
            list_valores.Add(new SelectListItem { Text = "FOSFATOS AMORFOS", Value = "FOSFATOS AMORFOS" });
            SelectList valores = new SelectList(list_valores, "Value", "Text", defaul);
            return valores;

        }








    }
}