using System;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using BolgerUtils.Framework.WebForms;
using Xunit;

namespace Tests.BolgerUtils.Framework.WebForms
{
    public class ExtensionUtilsTests
    {
        #region DropDownList

        [Fact]
        public void Test_SelectedDayOfWeekOrNull()
        {
            var dropDownList = new DropDownList();

            Assert.Null(dropDownList.SelectedDayOfWeekOrNull());
            dropDownList.Items.Add(new ListItem("--- Please select ---", string.Empty));
            Assert.Null(dropDownList.SelectedDayOfWeekOrNull());

            ListItem CreateListItem(DayOfWeek dayOfWeek) =>
                new ListItem(dayOfWeek.ToString(), ((int) dayOfWeek).ToString());

            dropDownList.Items.AddRange(new[]
            {
                CreateListItem(DayOfWeek.Monday), CreateListItem(DayOfWeek.Tuesday),
                CreateListItem(DayOfWeek.Wednesday), CreateListItem(DayOfWeek.Thursday),
                CreateListItem(DayOfWeek.Friday), CreateListItem(DayOfWeek.Saturday),
                CreateListItem(DayOfWeek.Sunday)
            });
            dropDownList.SelectedIndex = 0;
            Assert.Null(dropDownList.SelectedDayOfWeekOrNull());
            dropDownList.SelectedIndex++;
            Assert.Equal(DayOfWeek.Monday, dropDownList.SelectedDayOfWeekOrNull());
            dropDownList.SelectedIndex++;
            Assert.Equal(DayOfWeek.Tuesday, dropDownList.SelectedDayOfWeekOrNull());
            dropDownList.SelectedIndex++;
            Assert.Equal(DayOfWeek.Wednesday, dropDownList.SelectedDayOfWeekOrNull());
            dropDownList.SelectedIndex++;
            Assert.Equal(DayOfWeek.Thursday, dropDownList.SelectedDayOfWeekOrNull());
            dropDownList.SelectedIndex++;
            Assert.Equal(DayOfWeek.Friday, dropDownList.SelectedDayOfWeekOrNull());
            dropDownList.SelectedIndex++;
            Assert.Equal(DayOfWeek.Saturday, dropDownList.SelectedDayOfWeekOrNull());
            dropDownList.SelectedIndex++;
            Assert.Equal(DayOfWeek.Sunday, dropDownList.SelectedDayOfWeekOrNull());
            
            dropDownList.Items.Clear();
            dropDownList.Items.AddRange(new[]
            {
                CreateListItem(DayOfWeek.Monday), new ListItem("Test") { Selected = true }
            });
            Assert.Null(dropDownList.SelectedDayOfWeekOrNull());
            dropDownList.SelectedIndex--;
            Assert.Equal(DayOfWeek.Monday, dropDownList.SelectedDayOfWeekOrNull());
        }

        [Fact]
        public void Test_SelectedValueToIntOrNull()
        {
            var dropDownList = new DropDownList();

            Assert.Null(dropDownList.SelectedValueToIntOrNull());
            dropDownList.Items.Add(new ListItem("--- Please select ---", string.Empty));
            Assert.Null(dropDownList.SelectedValueToIntOrNull());

            dropDownList.Items.AddRange(new[] { new ListItem("1"), new ListItem("50"), new ListItem("100") });
            dropDownList.SelectedIndex = 0;
            Assert.Null(dropDownList.SelectedValueToIntOrNull());
            dropDownList.SelectedIndex++;
            Assert.Equal(1, dropDownList.SelectedValueToIntOrNull());
            dropDownList.SelectedIndex++;
            Assert.Equal(50, dropDownList.SelectedValueToIntOrNull());
            dropDownList.SelectedIndex++;
            Assert.Equal(100, dropDownList.SelectedValueToIntOrNull());
            dropDownList.SelectedIndex = 0;
            Assert.Null(dropDownList.SelectedValueToIntOrNull());

            dropDownList.Items.Clear();
            dropDownList.Items.AddRange(new[] { new ListItem("500"), new ListItem("Test") { Selected = true } });
            Assert.Null(dropDownList.SelectedValueToIntOrNull());
            dropDownList.SelectedIndex--;
            Assert.Equal(500, dropDownList.SelectedValueToIntOrNull());
        }

        #endregion

        #region HttpRequest

        [Fact]
        public void Test_BeginUrl() =>
            // Its pretty hard to test HttpRequest - this is a fake test.
            Assert.Throws<NullReferenceException>(() => HttpContext.Current.Request.BeginUrl());

        [Fact]
        public void Test_GetFormIntOrNull() =>
            // Its pretty hard to test HttpRequest - this is a fake test.
            Assert.Throws<NullReferenceException>(() => HttpContext.Current.Request.GetFormIntOrNull("ID"));

        [Fact]
        public void Test_GetFormValue() =>
            // Its pretty hard to test HttpRequest - this is a fake test.
            Assert.Throws<NullReferenceException>(() => HttpContext.Current.Request.GetFormValue("ID"));

        [Fact]
        public void Test_GetFormValueNotNull() =>
            // Its pretty hard to test HttpRequest - this is a fake test.
            Assert.Throws<NullReferenceException>(() => HttpContext.Current.Request.GetFormValueNotNull("ID"));

        #endregion

        #region Label

        [Fact]
        public void Test_HasError()
        {
            var label = new Label();
            Assert.True(label.Visible);
            Assert.Empty(label.Text);

            label.Visible = false;
            Assert.False(label.Visible);

            label.HasError(out var isFormInvalid);
            Assert.True(isFormInvalid);
            Assert.True(label.Visible);
            Assert.Empty(label.Text);

            label.Visible = false;
            Assert.False(label.Visible);

            label.HasError(out isFormInvalid, "Test");
            Assert.True(isFormInvalid);
            Assert.True(label.Visible);
            Assert.Equal("Test", label.Text);
        }

        #endregion

        #region Session

        [Fact]
        public void Test_Get() =>
            // Its pretty hard to test HttpSessionState - this is a fake test.
            Assert.Throws<NullReferenceException>(() => HttpContext.Current.Session.Get<int>("ID"));

        [Fact]
        public void Test_Set() =>
            // Its pretty hard to test HttpSessionState - this is a fake test.
            Assert.Throws<NullReferenceException>(() => HttpContext.Current.Session.Set("ID", 1));

        #endregion

        #region String

        [Fact]
        public void Test_ConnectionString() =>
            Assert.Equal(@"Server=.\SQLEXPRESS;Database=Test;Trusted_Connection=True;", "Test".ConnectionString());

        [Fact]
        public void Test_ConnectionStringSettings() =>
            Assert.IsType<ConnectionStringSettings>("Test".ConnectionStringSettings());

        #endregion

        #region TextBox

        [Fact]
        public void Test_ToIntOrNull()
        {
            var textBox = new TextBox();
            Assert.Null(textBox.ToIntOrNull());

            textBox.Text = "1";
            Assert.Equal(1, textBox.ToIntOrNull());

            textBox.Text = "50";
            Assert.Equal(50, textBox.ToIntOrNull());

            textBox.Text = "100";
            Assert.Equal(100, textBox.ToIntOrNull());

            textBox.Text = "0";
            Assert.Equal(0, textBox.ToIntOrNull());

            textBox.Text = "-5";
            Assert.Equal(-5, textBox.ToIntOrNull());

            textBox.Text = "Test";
            Assert.Null(textBox.ToIntOrNull());
        }

        [Fact]
        public void Test_ToTimeSpanOrNull()
        {
            var textBox = new TextBox();
            Assert.Null(textBox.ToTimeSpanOrNull());

            TimeSpan CreateTimeSpan(int hours, int minutes = 0) =>
                TimeSpan.FromHours(hours).Add(TimeSpan.FromMinutes(minutes));

            textBox.Text = "8:00 AM";
            Assert.Equal(CreateTimeSpan(8), textBox.ToTimeSpanOrNull());

            textBox.Text = "8:00 am";
            Assert.Equal(CreateTimeSpan(8), textBox.ToTimeSpanOrNull());

            textBox.Text = "08:00 AM";
            Assert.Equal(CreateTimeSpan(8), textBox.ToTimeSpanOrNull());

            textBox.Text = "08:30 AM";
            Assert.Equal(CreateTimeSpan(8, 30), textBox.ToTimeSpanOrNull());

            textBox.Text = "10:00 AM";
            Assert.Equal(CreateTimeSpan(10), textBox.ToTimeSpanOrNull());

            textBox.Text = "12:00 PM";
            Assert.Equal(CreateTimeSpan(12), textBox.ToTimeSpanOrNull());

            textBox.Text = "1:00 PM";
            Assert.Equal(CreateTimeSpan(13), textBox.ToTimeSpanOrNull());

            textBox.Text = "01:25 PM";
            Assert.Equal(CreateTimeSpan(13, 25), textBox.ToTimeSpanOrNull());

            textBox.Text = "14:59";
            Assert.Null(textBox.ToTimeSpanOrNull());
            Assert.Equal(CreateTimeSpan(14, 59), textBox.ToTimeSpanOrNull("HH:mm"));

            textBox.Text = "23:25:35";
            Assert.Null(textBox.ToTimeSpanOrNull());
            Assert.Equal(new TimeSpan(23, 25, 35), textBox.ToTimeSpanOrNull("HH:mm:ss"));

            textBox.Text = "Test";
            Assert.Null(textBox.ToTimeSpanOrNull());
        }

        #endregion
    }
}
