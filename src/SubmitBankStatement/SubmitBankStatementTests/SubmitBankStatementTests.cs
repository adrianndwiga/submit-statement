using NUnit.Framework;
using SubmitBankStatement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Moq;

namespace SubmitBankStatementTests
{
    [TestFixture]
    public class SubmitBankStatementTests
    {

        private SubmitBankStatement.SubmitBankStatement submitBankStatement;
        private Mock<IStore> mockStore;
        private Mock<IQueue> mockQueue;

        [SetUp]
        public void BeforeEach()
        {
            mockStore = new Mock<IStore>();
            mockQueue = new Mock<IQueue>();

            submitBankStatement = new SubmitBankStatement.SubmitBankStatement(mockStore.Object, mockQueue.Object);
        }

        [Test]
        public void SubmitBankStatement()
        {
            using (var reader = new StreamReader(@"../../../statement-01.csv"))
            {
                // Arrange
                var submittedBankStatement = new SubmittedBankStatement
                {
                    Name = "statement-01.csv",
                    Bank = Bank.Barclays,
                    AccountType = AccountType.PersonalCurrentAccount,
                    Content = reader.ReadToEnd()
                };
                var queueItem = new SubmittedBankStatementQueueItem(submittedBankStatement);
                mockStore.Setup(store => store.Write(submittedBankStatement.Name, submittedBankStatement.Content));
                mockQueue.Setup(queue => queue.Add(queueItem));

                // Act
                submitBankStatement.Submit(submittedBankStatement);

                // Assert
                mockStore.Verify(store => store.Write(submittedBankStatement.Name, submittedBankStatement.Content));
                Func<SubmittedBankStatementQueueItem, bool> AreEqual = actual => {
                    Assert.AreEqual(actual.Name, queueItem.Name);
                    Assert.AreEqual(actual.AccountType, queueItem.AccountType);
                    Assert.AreEqual(actual.Bank, queueItem.Bank);
                    return true;
                };  
                mockQueue.Verify(queue => queue.Add(It.Is<SubmittedBankStatementQueueItem>(actual => AreEqual(actual))));
            }
        }
    }
}
