using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace WebApp.Content {
    public static class Chtml {
        public static MvcHtmlString Control<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression) {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string dt = metadata.DataTypeName;
            MvcHtmlString control;
            switch (dt) {
                case "MultilineText":
                    control = Textarea(htmlHelper, expression, metadata);
                    break;
                case "Upload":
                    control = Textfile(htmlHelper, expression, metadata);
                    break;
                case "Date":
                    control = Textdate(htmlHelper, expression, metadata);
                    break;
                case "Password":
                    control = Textpass(htmlHelper, expression, metadata);
                    break;
                case "Checkmark":
                    control = Checkmark(htmlHelper, expression, metadata);
                    break;
                default:
                    control = Textbox(htmlHelper, expression, metadata);
                    break;
            }
            return MvcHtmlString.Create("<div class=\"form-group\">" + control.ToString() + "</div>");
        }
        public static MvcHtmlString Checkmark<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ModelMetadata metadata) {
            if (metadata.ModelType.Name == "Boolean") {
                var attr = new RouteValueDictionary { { "class", "form-check-input" } };

                var mvc1 = htmlHelper.CheckBoxFor((Expression<Func<TModel, bool>>)(object)expression, attr);
                attr = new RouteValueDictionary { { "class", "form-check-label" } };
                var mvc2 = mvcLabel(htmlHelper, expression, attr, metadata);

                var mvc3 = htmlHelper.ValidationMessageFor(expression);
                return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
            }
            else throw new Exception("Type of checkmark property must be boolean");
        }

        public static MvcHtmlString Textbox<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ModelMetadata metadata) {
            var attr = new RouteValueDictionary { { "class", "form-control tb" }, { "placeholder" ,metadata.Watermark} };
            var mvc1 = mvcLabel(htmlHelper, expression,  metadata);
            var mvc2 = htmlHelper.TextBoxFor(expression, attr);
            var mvc3 = htmlHelper.ValidationMessageFor(expression, " ");
            return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
        }

        public static MvcHtmlString Textarea<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ModelMetadata metadata) {
            var attr = new RouteValueDictionary { { "class", "form-control tb noresize" } };
            var mvc1 = mvcLabel(htmlHelper, expression, metadata);
            var mvc2 = htmlHelper.TextAreaFor(expression, attr);
            var mvc3 = htmlHelper.ValidationMessageFor(expression, " ");
            return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
        }
        public static MvcHtmlString Textfile<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ModelMetadata metadata) {
            var attr = new RouteValueDictionary { { "class", "form-control-file" }, { "type", "file" } };
            var mvc1 = mvcLabel(htmlHelper, expression, metadata);
            var mvc2 = htmlHelper.TextBoxFor(expression, attr);
            var mvc3 = htmlHelper.ValidationMessageFor(expression, " ");
            return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
        }
        public static MvcHtmlString Textdate<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ModelMetadata metadata) {
            var attr = new RouteValueDictionary { { "class", "form-control tb" }, { "type", "date" } };
            var mvc1 = mvcLabel(htmlHelper, expression,  metadata);
            var mvc2 = htmlHelper.TextBoxFor(expression, attr);
            var mvc3 = htmlHelper.ValidationMessageFor(expression, " ");
            return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
        }
        public static MvcHtmlString Textpass<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ModelMetadata metadata) {
            var attr = new RouteValueDictionary { { "class", "form-control tb" } };
            var mvc1 = mvcLabel(htmlHelper, expression, metadata);
            var mvc2 = htmlHelper.PasswordFor(expression, attr);
            var mvc3 = htmlHelper.ValidationMessageFor(expression, " ");
            return MvcHtmlString.Create(mvc1.ToString() + mvc2.ToString() + mvc3.ToString());
        }

        private static MvcHtmlString mvcLabel<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ModelMetadata metadata) {
            MvcHtmlString mvc2 = MvcHtmlString.Empty;
            var attr = new RouteValueDictionary { { "class", "control-label" }, { "style", "display:block" } };
            if (ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).IsRequired) {
                string label = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).DisplayName + "*";
                mvc2 = htmlHelper.LabelFor(expression, label, attr);
            }
            else {
                mvc2 = htmlHelper.LabelFor(expression, attr);
            }
            return mvc2;
        }
        private static MvcHtmlString mvcLabel<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,RouteValueDictionary attr, ModelMetadata metadata) {
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