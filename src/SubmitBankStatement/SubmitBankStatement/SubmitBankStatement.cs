using System;

namespace SubmitBankStatement
{
    public interface IStore {
        void Write(string fileName, string content);
    }
    public interface IQueue {
        void Add(SubmittedBankStatementQueueItem bankStatement);
    }

    public class SubmitBankStatement
    {
        private readonly IStore store;
        private readonly IQueue queue;

        public SubmitBankStatement(IStore store, IQueue queue)
        {
            this.store = store;
            this.queue = queue;
        }

        public void Submit(SubmittedBankStatement bankStatement) {
            store.Write(bankStatement.Name, bankStatement.Content);
            queue.Add(new SubmittedBankStatementQueueItem(bankStatement));
        }
    }
}
