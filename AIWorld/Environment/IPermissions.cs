using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AIWorld.Environment
{
    interface IPermissions<TRepresentation>
    {
        bool CheckPermissionName(string permission);

        List<string> GetEnabledPermissions(TRepresentation permissions);

        List<string> GetDisabledPermissions(TRepresentation permissions);

        TRepresentation Flip(TRepresentation currentPermissions, string targetPermission);

        TRepresentation SetOn(TRepresentation currentPermissions, string targetPermission);

        TRepresentation SetOff(TRepresentation currentPermissions, string targetPermission);
    }

    public class BytePermissions : IPermissions<byte>
    {
        private byte count;
        private string[] permissionNames;
        private Dictionary<string, byte> permissionMasks;

        public BytePermissions(List<string> permissions)
        {
            this.permissionNames = new string[permissions.Count];
            permissionMasks = new Dictionary<string, byte>();
            for (byte i = 0; i < permissions.Count; i ++)
            {
                this.permissionNames[i] = permissions[i];
                permissionMasks.Add(permissions[i], (byte)(1 << i));
            }
        }

        public bool CheckPermissionName(string permission)
        {
            return permissionMasks.ContainsKey(permission);
        }

        public string[] GetPermissionNames()
        {
            return permissionNames;
        }

        public List<string> GetEnabledPermissions(byte permissions)
        {
            List<string> returnList = new List<string>();
            for (byte i = 0; i < this.permissionNames.Length; i++)
            {
                if (((permissions >> i) & 1) == 1)
                {
                    returnList.Add(permissionNames[i]);
                }
            }
            return returnList;
        }

        public List<string> GetDisabledPermissions(byte permissions)
        {
            List<string> returnList = new List<string>();
            for (byte i = 0; i < this.permissionNames.Length; i++)
            {
                if (((permissions >> i) & 1) == 0)
                {
                    returnList.Add(permissionNames[i]);
                }
            }
            return returnList;
        }

        public byte Flip(byte currentPermissions, string targetPermission)
        {
            byte mask = permissionMasks[targetPermission];
            return (byte)(currentPermissions ^ mask);
        }

        public byte SetOff(byte currentPermissions, string targetPermission)
        {
            byte mask = permissionMasks[targetPermission];
            return (byte)(currentPermissions & ~mask);
        }

        public byte SetOn(byte currentPermissions, string targetPermission)
        {
            byte mask = permissionMasks[targetPermission];
            return (byte)(currentPermissions | mask);
        }
    }
}
