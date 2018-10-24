using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace WebApp.Content {
    public static class Chtml {
        public static MvcHtmlString Control<TModel, TProperty>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression) {
            string dt = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).DataTypeName;
            switch (dt) {
                case "MultilineText":
                    return Textarea(htmlHelper, expression);
                case "Upload":
                    return Textfile(htmlHelper, expression);
                case "Date":
                    return Textdate(htmlHelper, expression);
                case "Password":
                    return Textpass(htmlHelper, expression);
                default:
                    return Textbox(htmlHelper, expression);
            }
        }
        public static MvcHtmlString Textbox<TModel, TProperty>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression) {
            var attr = new RouteValueDictionary { { "class", "form-control tb" } };
            var mvc1 = htmlHelper.TextBoxFor(expression, attr);
            attr = new RouteValueDictionary { { "class", "control-label pl-3 pb-3" } };
            var mvc2 = mvcLabel(htmlHelper, expression, attr);
            var mvc3 = htmlHelper.ValidationMessageFor(expression, " ");
            return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
        }

        public static MvcHtmlString Textarea<TModel, TProperty>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression) {
            var attr = new RouteValueDictionary { { "class", "form-control tb noresize" } };
            var mvc1 = htmlHelper.TextAreaFor(expression, attr);
            attr = new RouteValueDictionary { { "class", "control-label pl-3 pb-3" } };
            var mvc2 = mvcLabel(htmlHelper, expression, attr);
            var mvc3 = htmlHelper.ValidationMessageFor(expression, " ");
            return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
        }
        public static MvcHtmlString Textfile<TModel, TProperty>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression) {
            var attr = new RouteValueDictionary { { "class", "form-control-file" }, { "type", "file" } };
            var mvc1 = htmlHelper.TextBoxFor(expression, attr);
            attr = new RouteValueDictionary { { "class", "control-label pl-3 pb-3" } };
            var mvc2 = mvcLabel(htmlHelper, expression, attr);
            var mvc3 = htmlHelper.ValidationMessageFor(expression, " ");
            return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
        }
        public static MvcHtmlString Textdate<TModel, TProperty>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression) {
            var attr = new RouteValueDictionary { { "class", "form-control tb" }, { "type", "date" } };
            var mvc1 = htmlHelper.TextBoxFor(expression, attr);
            attr = new RouteValueDictionary { { "class", "control-label pl-3 pb-3" } };
            var mvc2 = mvcLabel(htmlHelper, expression, attr);
            var mvc3 = htmlHelper.ValidationMessageFor(expression, " ");
            return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
        }
        public static MvcHtmlString Textpass<TModel, TProperty>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression) {
            var attr = new RouteValueDictionary { { "class", "form-control tb" } };
            var mvc1 = htmlHelper.PasswordFor(expression, attr);
            attr = new RouteValueDictionary { { "class", "control-label pl-3 pb-3" } };
            var mvc2 = mvcLabel(htmlHelper, expression, attr);
            var mvc3 = htmlHelper.ValidationMessageFor(expression, " ");
            return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
        }

        private static MvcHtmlString mvcLabel<TModel, TProperty>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression, RouteValueDictionary attr) {
            MvcHtmlString mvc2 = MvcHtmlString.Empty;
            if (ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).IsRequired) {
                string label = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).DisplayName + "*";
                mvc2 = htmlHelper.LabelFor(expression, label, attr);
            }
            else {
                mvc2 = htmlHelper.LabelFor(expression, attr);
            }
            return mvc2;
        }
    }
}