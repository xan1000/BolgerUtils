using System.Web.UI.WebControls;
using BolgerUtils.Framework.WebForms;
using Xunit;

namespace Tests.BolgerUtils.Framework.WebForms
{
    public class UtilsTests
    {
        [Fact]
        public void TrimTextTest()
        {
            const string text1 = "Hello", text2 = "World   ", text3 = "   Test", text4 = "   Hello World Test   ";

            var textBox1 = new TextBox { Text = text1 };
            var textBox2 = new TextBox { Text = text2 };
            var textBox3 = new TextBox { Text = text3 };
            var textBox4 = new TextBox { Text = text4 };

            Assert.Equal(text1, textBox1.Text);
            Assert.Equal(text2, textBox2.Text);
            Assert.Equal(text3, textBox3.Text);
            Assert.Equal(text4, textBox4.Text);

            Utils.TrimText(textBox1, textBox2, textBox3, textBox4);

            Assert.Equal(text1, textBox1.Text);
            Assert.Equal(text2.Trim(), textBox2.Text);
            Assert.NotEqual(text2, textBox2.Text);
            Assert.Equal(text3.Trim(), textBox3.Text);
            Assert.NotEqual(text3, textBox3.Text);
            Assert.Equal(text4.Trim(), textBox4.Text);
            Assert.NotEqual(text4, textBox4.Text);
        }
    }
}
