using System;

namespace App1
{
    public class AlarmRecord
    {
        public DateTime time;
        public int id;
        public State state;

        public DateTime nextRealarmTime;

        public enum State
        {
            Armed,
            WaitingUserResponse,
            Disarmed
        }

        public AlarmRecord(DateTime time, int id)
        {
            this.time = time;
            this.id = id;
            nextRealarmTime = time;
        }
        public void Skip()
        {
            nextRealarmTime = DateTime.Now.AddSeconds(5);
            state = State.Armed;
        }
        public void Disarm()
        {
            nextRealarmTime = time;
            state = State.Disarmed;
        }
    }
}
