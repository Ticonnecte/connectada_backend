
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Zenite.Pi.Entities.Enums;
using Zenite.Pi.Entities.Validation;
using Zenite.Pi.Web;

namespace CidConnectada.Website.Code.Extensions
{
    public static class HtmlHelperHieExtensions
    {
        /// <summary>
        /// Render all messages that have been set during execution of the controller action. 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static HtmlString RenderMessages(this HtmlHelper htmlHelper)
        {
            StringBuilder mensagem = new StringBuilder();
            foreach (var tipoMensagem in Enum.GetNames(typeof(MessageType)))
            {
                var message = htmlHelper.ViewContext.ViewData.ContainsKey(tipoMensagem)
                                ? htmlHelper.ViewContext.ViewData[tipoMensagem]
                                : htmlHelper.ViewContext.TempData.ContainsKey(tipoMensagem)
                                    ? htmlHelper.ViewContext.TempData[tipoMensagem]
                                    : null;
                if (message != null)
                {
                    mensagem.AppendFormat("<div class='alert alert-{0}'>", tipoMensagem.ToLowerInvariant());
                    mensagem.Append("<button type='button' class='close' data-dismiss='alert'>×</button>");

                    if (message as IEnumerable<string> != null)
                    {
                        var mensagens = message as IEnumerable<string>;
                        foreach (var item in mensagens)
                        {
                            mensagem.AppendFormat("<p>{0}</p>", item);
                        }
                    }
                    else
                    {
                        mensagem.AppendFormat("<p>{0}</p>", message);
                    }
                    mensagem.Append("</div>");
                }
            }
            return MvcHtmlString.Create(mensagem.ToString());
        }

        public static HtmlString RenderMessages(this HtmlHelper htmlHelper, MessageType messageType, string message)
        {
            StringBuilder mensagem = new StringBuilder();
            foreach (var tipoMensagem in Enum.GetNames(typeof(MessageType)))
            {
                if (message != null)
                {
                    mensagem.AppendFormat("<div class='alert alert-{0}'>", Enum.GetName(typeof(MessageType), messageType).ToLowerInvariant());
                    mensagem.Append("<button type='button' class='close' data-dismiss='alert'>×</button>");
                    mensagem.AppendFormat("<p>{0}</p>", message);
                    mensagem.Append("</div>");
                }
            }
            return MvcHtmlString.Create(mensagem.ToString());
        }

        public static HtmlString TitleForPopup(this HtmlHelper html, String title)
        {
            TagBuilder tagBuilder = new TagBuilder("span");
            tagBuilder.AddCssClass("popupTitle");
            tagBuilder.InnerHtml = title;
            return new HtmlString(tagBuilder.ToString());
        }

        public static MvcHtmlString VoltarButton(this HtmlHelper helper)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", "javascript:history.go(-1)");
            builder.InnerHtml = "Voltar";

            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString GridRowCheckTemplate(this HtmlHelper helper, string propertyName, string gridName,
            string checkColumnName, string checkClassName)
        {
            var scriptBuilder = new TagBuilder("script");
            scriptBuilder.MergeAttribute("type", "text/javascript");

            string showEvent = "$('#{0}').show(function () {{\ndebugger;";
            showEvent += "  var dataItem = $('#{1}').data(\"kendoGrid\").dataItem($(this).closest('tr'));\n";
            showEvent += "  var disabled = !dataItem['{2}'];\n";
            showEvent += "  $(this).prop('disabled', disabled);\n";
            showEvent += "}});\n\n";

            scriptBuilder.InnerHtml = string.Format(showEvent, propertyName, gridName, checkColumnName);

            string clickEvent = "$('.{0}').click(function () {{\ndebugger;";
            clickEvent += " if (!$(this).prop('checked')) {{\n";
            clickEvent += "     $(this).closest('tr').find('.k-tooltip-validation').remove();\n";
            clickEvent += "     $('#{1}').data(\"kendoGrid\").closeCell();\n";
            clickEvent += " }}\n";
            clickEvent += "}});\n\n";

            scriptBuilder.InnerHtml += string.Format(clickEvent, checkClassName, gridName);
            return MvcHtmlString.Create(scriptBuilder.ToString());
        }

        ///// <summary>
        ///// Retorna um elemento input HTML para uma propriedade do objeto representado 
        ///// pela expressão System.Linq.Expressions.Expression.
        ///// </summary>
        ///// <typeparam name="TModel">O tipo do model</typeparam>
        ///// <typeparam name="TProperty">O tipo do valor</typeparam>
        ///// <param name="htmlHelper">Instância do HtmlHelper que esse método extende</param>
        ///// <param name="expression">Uma expressão que identifica o propriedade do model</param>
        ///// <param name="htmlAttributes">Um objeto anônimo que pode conter dados de visualização adicionais 
        ///// <returns>Um elemento input HTML para a propriedade do objeto representado</returns>
        //public static MvcHtmlString InputFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        //{
        //    var attributes = GetAllAttributes(htmlHelper, expression, htmlAttributes);

        //    ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
        //    string format = metadata.DisplayFormatString ?? string.Empty;

        //    return htmlHelper.TextBoxFor(expression, format, attributes);
        //}

        private static IDictionary<string, object> GetAllAttributes<TModel, TProperty>(HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var allAttributes = GetAttributes(htmlHelper, expression);
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            foreach (var attribute in attributes)
            {
                if (allAttributes.ContainsKey(attribute.Key))
                {
                    allAttributes[attribute.Key] = attribute.Value;
                }
                else
                {
                    allAttributes.Add(attribute.Key, attribute.Value);
                }
            }
            return allAttributes;
        }

        private static IDictionary<string, object> GetAttributes<TModel, TProperty>(HtmlHelper<TModel> htmlHelper,
    Expression<Func<TModel, TProperty>> expression)
        {
            var member = expression.Body as MemberExpression;
            var id = ExpressionHelper.GetExpressionText(expression);
            var validators = member.Member.GetCustomAttributes(false);
            var htmlAttributes = htmlHelper.GetUnobtrusiveValidationAttributes(id);
            var model = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData).Model;
            var value = Convert.ToString(model);

            if (validators != null)
            {
                foreach (var validator in validators)
                {
                    if (validator is StringLengthAttribute)
                    {
                        var attribute = validator as StringLengthAttribute;
                        htmlAttributes.Add("maxLength", attribute.MaximumLength);
                    }
                    else if (validator is NumericAttribute)
                    {
                        var attribute = validator as NumericAttribute;
                        htmlAttributes.Add("type", "numerico");
                        htmlAttributes.Add("maxLength", attribute.MaxLength);
                    }
                    else if (validator is DecimalAttribute)
                    {
                        var attribute = validator as DecimalAttribute;
                        htmlAttributes.Add("type", "decimal");
                        htmlAttributes.Add("limit", attribute.Limit);
                        htmlAttributes.Add("scale", attribute.Scale);
                    }
                    else if (validator is DateTimeAttribute)
                    {
                        var attribute = validator as DateTimeAttribute;

                        if (model != null)
                        {
                            switch (attribute.DateTimeType)
                            {
                                case DateTimeType.Data:
                                    value = string.Format("{0:dd/MM/yyyy}", model);
                                    break;
                                case DateTimeType.DataHora:
                                    value = string.Format("{0:dd/MM/yyyy HH:mm:ss}", model);
                                    break;
                                case DateTimeType.Competencia:
                                    value = string.Format("{0:MM/yyyy}", model);
                                    break;
                                default:
                                    break;
                            }
                        }

                        htmlAttributes.Add("type", attribute.DateTimeType.ToString().ToLower());
                    }
                }
            }

            htmlAttributes.Add("value", value);

            return htmlAttributes;
        }


        /// <summary>
        /// Retorna um elemento input HTML do tipo submit.
        /// </summary>
        /// <param name="htmlHelper">Instância do HtmlHelper que esse método extende</param>
        /// <param name="value">Valor exibido no botão</param>
        /// <param name="name">Nome do botão</param>
        /// <param name="rendered">Indica se o input será renderizado</param>
        /// <param name="disabled">Infica se o input será desabilitado</param>
        /// <param name="cssClass">Classe css atribuida ao input</param>
        /// <param name="confirmation">Nome da action de confirmação</param>
        /// <param name="cssClass">Atributos a serem incluidos no html</param>
        /// <returns>Retorna um elemento input HTML do tipo submit</returns>
        public static MvcHtmlString Button(this HtmlHelper htmlHelper, string value, string name = null,
            bool rendered = true, bool disabled = false, string cssClass = null, string confirmation = null,
            object htmlAttributes = null)
        {
            var htmlString = string.Empty;
            if (rendered)
            {
                var tagBuilder = new TagBuilder("input");
                tagBuilder.Attributes["type"] = "submit";
                tagBuilder.Attributes["value"] = value;
                tagBuilder.Attributes["id"] = value;

                if (name != null)
                {
                    tagBuilder.Attributes["id"] = name;
                    tagBuilder.Attributes["name"] = name;
                }

                if (disabled)
                {
                    tagBuilder.Attributes["disabled"] = "disabled";
                }

                if (cssClass != null)
                {
                    tagBuilder.AddCssClass(cssClass);
                }

                if (!string.IsNullOrEmpty(confirmation))
                {
                    tagBuilder.Attributes["confirmation"] = confirmation;
                }

                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                foreach (var attribute in attributes)
                {
                    tagBuilder.Attributes[attribute.Key] = attribute.Value.ToString();
                }

                htmlString = tagBuilder.ToString(TagRenderMode.SelfClosing);
            }
            return MvcHtmlString.Create(htmlString);
        }

        //public static HtmlString Paginator(this HtmlHelper html, IPagedResult model)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    int resultsByPage = Convert.ToInt32(ApplicationContext.AppSettings["pagination.resultsByPage"]);
        //    int countPagesDisplay = Convert.ToInt32(ApplicationContext.AppSettings["pagination.countPagesDisplay"]);

        //    if (model.Quantity == default(int))
        //    {
        //        var tagBuilder = new TagBuilder("span");
        //        tagBuilder.SetInnerText(ResourceMessage.NenhumResultadoEncontrado);
        //        sb.Append(tagBuilder.ToString());
        //    }
        //    // somente exibir caso a quantidade total de itens for maior que a quantidade de itens por página.
        //    else if (model.Quantity > resultsByPage)
        //    {
        //        int ultimaPagina = (model.Quantity / resultsByPage) + 1;
        //        int numeroPagina = 1;
        //        bool existePrimeriaPagina = model.CurrentPage > 3 && ultimaPagina > countPagesDisplay;
        //        bool existePaginaAnterior = model.CurrentPage > 1 && ultimaPagina > countPagesDisplay;
        //        bool existeUltimaPagina = model.CurrentPage < ultimaPagina - 2 && ultimaPagina > countPagesDisplay;
        //        bool existeProximaPagina = model.CurrentPage < ultimaPagina && ultimaPagina > countPagesDisplay;

        //        if (model.CurrentPage > 2 && ultimaPagina > countPagesDisplay)
        //        {
        //            numeroPagina = ultimaPagina - (ultimaPagina - model.CurrentPage) - 2;
        //        }

        //        if (ultimaPagina == model.CurrentPage && ultimaPagina > countPagesDisplay)
        //        {
        //            numeroPagina = ultimaPagina - (countPagesDisplay - 1);
        //        }

        //        sb.Append("<ul>");
        //        if (existePrimeriaPagina)
        //        {
        //            sb.Append(CriarPagina(1, "Primeira"));
        //        }

        //        if (existePaginaAnterior)
        //        {
        //            sb.Append(CriarPagina(model.CurrentPage - 1, "&lt;&lt;"));
        //        }

        //        for (int i = 0; i < countPagesDisplay && numeroPagina <= ultimaPagina; i++)
        //        {
        //            if (numeroPagina == model.CurrentPage)
        //            {
        //                sb.Append("<li>");
        //                sb.Append("<span>").Append((numeroPagina).ToString()).Append("</span>");
        //                sb.Append("</li>");
        //            }
        //            else
        //            {
        //                sb.Append(CriarPagina(numeroPagina, numeroPagina.ToString()));
        //            }
        //            numeroPagina++;
        //        }

        //        if (existeProximaPagina)
        //        {
        //            sb.Append(CriarPagina(model.CurrentPage + 1, "&gt;&gt;"));
        //        }

        //        if (existeUltimaPagina)
        //        {
        //            sb.Append(CriarPagina(ultimaPagina, "Último"));
        //        }

        //        sb.Append("</ul>");
        //    }

        //    return MvcHtmlString.Create(sb.ToString());
        //}


        //private static string CriarPagina(int numeroPagina, string texto = "")
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendFormat("<li><a pagina='{0}' href=\"javascript:void();\">", numeroPagina);
        //    sb.AppendFormat("{0}</a></li>", texto);
        //    return sb.ToString();
        //}



    }
}