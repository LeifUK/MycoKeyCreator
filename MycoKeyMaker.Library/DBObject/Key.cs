using System;
using PetaPoco.NetCore;

namespace MycoKeyMaker.Library.DBObject
{
    [TableName(Database.TableNames.Key)]
    public class Key : IObject
    {
        public Int64 id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string copyright { get; set; }
        public Int16 flags { get; set; }

        // Flags
        private enum Flags
        {
            Publish = 0x0001
        }

        [Ignore]
        public bool Publish
        {
            get
            {
                return ((int)flags & (int)Library.DBObject.Key.Flags.Publish) == (int)Library.DBObject.Key.Flags.Publish;
            }
            set
            {
                if (value)
                {
                    flags = (short)((int)flags | (int)Library.DBObject.Key.Flags.Publish);
                }
                else
                {
                    flags = (short)((int)flags & ~(int)Library.DBObject.Key.Flags.Publish);
                }
            }
        }
    }
}
