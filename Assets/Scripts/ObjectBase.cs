using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster
{
    public abstract class ObjectBase : MonoBehaviour
    {
        // Object ID
        private int OID;
        public abstract ObjectInfoBase Info { get; set; }
    }
}