﻿using OpenQA.Selenium;

namespace Atata
{
    public class FindByLabelStrategy : IComponentScopeLocateStrategy
    {
        public ComponentScopeLocateResult Find(IWebElement scope, ComponentScopeLocateOptions options, SearchOptions searchOptions)
        {
            string labelXPath = new ComponentScopeXPathBuilder(options).
                WrapWithIndex(x => x.Descendant._("label")[y => y.TermsConditionOfContent]);

            IWebElement label = scope.Get(By.XPath(labelXPath).With(searchOptions).Label(options.GetTermsAsString()));

            if (label == null)
            {
                if (searchOptions.IsSafely)
                    return new MissingComponentScopeLocateResult();
                else
                    throw ExceptionFactory.CreateForNoSuchElement(options.GetTermsAsString(), searchContext: scope);
            }

            string elementId = label.GetAttribute("for");
            IdXPathForLabelAttribute idXPathForLabelAttribute;

            if (string.IsNullOrEmpty(elementId))
            {
                return new SequalComponentScopeLocateResult(label, new FindFirstDescendantStrategy());
            }
            else if ((idXPathForLabelAttribute = options.Metadata.Get<IdXPathForLabelAttribute>(AttributeLevels.Component)) != null)
            {
                ComponentScopeLocateOptions idOptions = options.Clone();
                idOptions.Terms = new[] { idXPathForLabelAttribute.XPathFormat.FormatWith(elementId) };
                idOptions.Index = null;

                return new SequalComponentScopeLocateResult(scope, new FindByXPathStrategy(), idOptions);
            }
            else
            {
                ComponentScopeLocateOptions idOptions = options.Clone();
                idOptions.Terms = new[] { elementId };
                idOptions.Index = null;
                idOptions.Match = TermMatch.Equals;

                return new SequalComponentScopeLocateResult(scope, new FindByIdStrategy(), idOptions);
            }
        }
    }
}
