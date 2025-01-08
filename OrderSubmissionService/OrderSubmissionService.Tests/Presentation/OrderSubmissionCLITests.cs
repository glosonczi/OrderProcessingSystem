using Moq;
using OrderSubmissionService.Communication;
using OrderSubmissionService.Presentation;

namespace OrderSubmissionService.Tests.Presentation
{
    [TestClass]
    public class OrderSubmissionCLITests
    {
        private readonly string _usageMessage = "Usage: dotnet run -- order \"CustomerName\" \"Product\"{0}";
        private readonly string _emptyArgumentMessage = "Customer name and product cannot be empty!{0}";
        private readonly string _orderSubmittedMessage = "Order {1} submitted.{0}";
        protected readonly Mock<IMqttOrderPublisher> mockMqttOrderPublisher = new();

        [TestMethod]
        public void HandleSubmission_WhenInvalidArguments_ThenWriteWarningMessages_AndWhenValidArguments_ThenSubmitOrder()
        {
            // Arrange
            var orderSubmissionCLI = new OrderSubmissionCLI(mockMqttOrderPublisher.Object);
            var output = new StringWriter();
            Console.SetOut(output);
            string validCustomer = "testCustomerName";
            string validProduct = "testProduct";
            string[] args0 = { "order", validCustomer };
            string[] args1 = { "purchase", validCustomer, validProduct };
            string[] args2 = { "order", validCustomer, "    " };
            string[] args3 = { "order", validCustomer, validProduct };
            string orderId = "0";
            mockMqttOrderPublisher.Setup(mop => mop.PublishOrder(It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => Console.WriteLine(string.Format(_orderSubmittedMessage, "", orderId)));

            // Act
            orderSubmissionCLI.HandleSubmission(args0);
            orderSubmissionCLI.HandleSubmission(args1);
            orderSubmissionCLI.HandleSubmission(args2);
            orderSubmissionCLI.HandleSubmission(args3);

            // Assert
            Assert.AreEqual(
                string.Format($"{_usageMessage}{_usageMessage}{_emptyArgumentMessage}{_orderSubmittedMessage}", Environment.NewLine, orderId),
                output.ToString()
                );
        }
    }
}
