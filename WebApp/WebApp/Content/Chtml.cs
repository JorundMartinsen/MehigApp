using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using WebApp.Models.Documents;

namespace WebApp.Content {
    public static class Chtml {
        public static MvcHtmlString Control<TModel, TProperty>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression) {
            Type t = expression.ReturnType;
            if (t == typeof(HttpPostedFileBase)) {
                return Textfile(htmlHelper, expression);
            }
            else if (t==typeof(DateTime)) {
                return Textdate(htmlHelper, expression);
            }
            //else if (t==typeof(BigString)) {
            //    return Textarea(htmlHelper, expression);
            //}
            else {
                return Textbox(htmlHelper, expression);
            }
        }
            public static MvcHtmlString Textbox<TModel, TProperty>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression) {
            var attr = new RouteValueDictionary { { "class", "form-control tb" } };
            var mvc1 = htmlHelper.TextBoxFor(expression, attr);
            attr = new RouteValueDictionary { { "class", "control-label pl-3 pb-3" } };
            var mvc2 = htmlHelper.LabelFor(expression, attr);
            var mvc3 = htmlHelper.ValidationMessageFor(expression, "");
            return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
        }

        public static MvcHtmlString Textarea<TModel, TProperty>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression) {
            var attr = new RouteValueDictionary { { "class", "form-control tb noresize" } };
            var mvc1 = htmlHelper.TextAreaFor(expression, attr);
            attr = new RouteValueDictionary { { "class", "control-label pl-3 pb-3" } };
            var mvc2 = htmlHelper.LabelFor(expression, attr);
            var mvc3 = htmlHelper.ValidationMessageFor(expression, "");
            return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
        }
        public static MvcHtmlString Textfile<TModel, TProperty>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression) {
            var attr = new RouteValueDictionary { { "class", "form-control-file" }, { "type", "file" } };
            var mvc1 = htmlHelper.TextBoxFor(expression, attr);
            attr = new RouteValueDictionary { { "class", "control-label pl-3 pb-3" } };
            var mvc2 = htmlHelper.LabelFor(expression, attr);
            var mvc3 = htmlHelper.ValidationMessageFor(expression, "");
            return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
        }
        public static MvcHtmlString Textdate<TModel, TProperty>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression) {
            var attr = new RouteValueDictionary { { "class", "form-control tb" }, { "type", "date" } };
            var mvc1 = htmlHelper.TextBoxFor(expression, attr);
            attr = new RouteValueDictionary { { "class", "control-label pl-3 pb-3" } };
            var mvc2 = htmlHelper.LabelFor(expression, attr);
            var mvc3 = htmlHelper.ValidationMessageFor(expression, "");
            return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
        }
    }
}