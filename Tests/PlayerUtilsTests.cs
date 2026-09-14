using NUnit.Framework;

namespace ValheimRcon.Tests
{
    [TestFixture]
    public class PlayerUtilsTests
    {
        [Test]
        public void FormatPlayerNameAndId_WithPlayerName_ShowsNameAndId()
        {
            string result = PlayerUtils.FormatPlayerNameAndId(193029L, "Ragnar");

            Assert.AreEqual("Ragnar (193029)", result);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void FormatPlayerNameAndId_WithoutPlayerName_ShowsId(string playerName)
        {
            string result = PlayerUtils.FormatPlayerNameAndId(193029L, playerName);

            Assert.AreEqual("193029", result);
        }
    }
}
