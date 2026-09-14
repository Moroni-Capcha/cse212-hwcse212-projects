using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Enqueue "Low"(1), "High"(5), "Medium"(3). Dequeue three times.
    // Expected Result: High, Medium, Low
    // Defect(s) Found: The Dequeue loop condition `index < _queue.Count - 1` skips the
    // last element in the list, so it can never be selected as the highest priority item.
    public void TestPriorityQueue_DequeueInPriorityOrder()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Low", 1);
        priorityQueue.Enqueue("High", 5);
        priorityQueue.Enqueue("Medium", 3);

        Assert.AreEqual("High", priorityQueue.Dequeue());
        Assert.AreEqual("Medium", priorityQueue.Dequeue());
        Assert.AreEqual("Low", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Enqueue "A"(2), "B"(5), "C"(5), "D"(1). Dequeue four times.
    // Expected Result: B, C, A, D  (B and C tie at priority 5; B was enqueued first, so it
    // must come out first per FIFO tie-breaking)
    // Defect(s) Found: Dequeue uses `>=` when comparing priorities, so on a tie it keeps
    // updating to the later index and returns the LAST item with the highest priority
    // instead of the first (violates FIFO tie-breaking requirement).
    public void TestPriorityQueue_TieBreaksByFIFO()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 2);
        priorityQueue.Enqueue("B", 5);
        priorityQueue.Enqueue("C", 5);
        priorityQueue.Enqueue("D", 1);

        Assert.AreEqual("B", priorityQueue.Dequeue());
        Assert.AreEqual("C", priorityQueue.Dequeue());
        Assert.AreEqual("A", priorityQueue.Dequeue());
        Assert.AreEqual("D", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Enqueue "Only"(1) and dequeue it, then dequeue again on the now-empty queue.
    // Expected Result: First Dequeue returns "Only"; second Dequeue throws
    // InvalidOperationException with message "The queue is empty."
    // Defect(s) Found: Dequeue never removes the selected item from the internal list
    // (missing a RemoveAt call), so the same item can be returned repeatedly and the queue
    // never actually empties.
    public void TestPriorityQueue_DequeueRemovesItemAndEmptyThrows()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("Only", 1);

        Assert.AreEqual("Only", priorityQueue.Dequeue());

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
    }

    [TestMethod]
    // Scenario: Call Dequeue on a brand-new, empty queue.
    // Expected Result: InvalidOperationException with message "The queue is empty."
    // Defect(s) Found: None for this specific check — the empty-queue guard clause itself
    // was already implemented correctly.
    public void TestPriorityQueue_EmptyQueueThrows()
    {
        var priorityQueue = new PriorityQueue();

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException e)
        {
            Assert.AreEqual("The queue is empty.", e.Message);
        }
    }
}