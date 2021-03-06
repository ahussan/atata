﻿using NUnit.Framework;
using OpenQA.Selenium;

namespace Atata.Tests
{
    public class CheckBoxListTest : AutoTest
    {
        private CheckBoxListPage page;

        protected override void OnSetUp()
        {
            page = Go.To<CheckBoxListPage>();
        }

        [Test]
        public void CheckBoxList_Enum()
        {
            page.ByIdAndLabel.Should.Equal(CheckBoxListPage.Options.None);

            SetAndVerifyValues(
                page.ByIdAndLabel,
                CheckBoxListPage.Options.OptionC | CheckBoxListPage.Options.OptionD,
                CheckBoxListPage.Options.OptionB);

            SetAndVerifyValues(
                page.ByXPathAndValue,
                CheckBoxListPage.Options.OptionA,
                CheckBoxListPage.Options.OptionsDF);

            page.ByIdAndLabel.Should.Not.HaveChecked(CheckBoxListPage.Options.OptionA).
                ByIdAndLabel.Should.HaveChecked(CheckBoxListPage.Options.OptionD | CheckBoxListPage.Options.OptionF);

            SetAndVerifyValues(
                page.ByXPathAndValue,
                CheckBoxListPage.Options.None,
                CheckBoxListPage.Options.OptionA);

            page.ByIdAndLabel.Check(CheckBoxListPage.Options.OptionD);
            page.ByXPathAndValue.Should.Equal(CheckBoxListPage.Options.OptionA | CheckBoxListPage.Options.OptionD);

            page.ByXPathAndValue.Uncheck(CheckBoxListPage.Options.OptionA);
            page.ByIdAndLabel.Should.HaveChecked(CheckBoxListPage.Options.OptionD);

            Assert.Throws<AssertionException>(() =>
                page.ByIdAndLabel.Should.AtOnce.Not.HaveChecked(CheckBoxListPage.Options.OptionD));

            Assert.Throws<AssertionException>(() =>
                page.ByIdAndLabel.Should.AtOnce.HaveChecked(CheckBoxListPage.Options.OptionA));

            Assert.Throws<NoSuchElementException>(() =>
                page.ByIdAndLabel.Set(CheckBoxListPage.Options.MissingValue));
        }
    }
}
