using System;

namespace App1
{
    public class AlarmRecord
    {
        public DateTime time;
        public State state;

        public DateTime nextRealarmTime;

        public enum State
        {
            Armed,
            WaitingUserResponse,
            Disarmed
        }

        public AlarmRecord(DateTime time)
        {
            this.time = time;
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
