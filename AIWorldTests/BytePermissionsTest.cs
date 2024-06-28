using AIWorld.Environment;

using System.Security;
using System.Text;

namespace AIWorldTests
{
    public class BytePermissionsTest
    {
        [Fact]
        public void SetOnHardCoded()
        {
            List<String> permissionNames = new List<String>();
            permissionNames.Add("Sight");
            permissionNames.Add("XYTheta");
            BytePermissions permissions = new BytePermissions(permissionNames);

            byte currentPermissions = 0;
            currentPermissions = permissions.SetOn(currentPermissions, "Sight");
            Assert.True(currentPermissions == 1);
            currentPermissions = permissions.SetOn(currentPermissions, "XYTheta");
            Assert.True(currentPermissions == 3);
        }

        [Fact]
        public void SetOffHardCoded()
        {
            List<String> permissionNames = new List<String>();
            permissionNames.Add("Sight");
            permissionNames.Add("XYTheta");
            BytePermissions permissions = new BytePermissions(permissionNames);

            byte currentPermissions = 0;
            currentPermissions = permissions.SetOn(currentPermissions, "Sight");
            currentPermissions = permissions.SetOn(currentPermissions, "XYTheta");

            currentPermissions = permissions.SetOff(currentPermissions, "Sight");
            Assert.True(currentPermissions == 2);
            currentPermissions = permissions.SetOff(currentPermissions, "XYTheta");
            Assert.True(currentPermissions == 0);
        }

        [Fact]
        public void SetOffOnRandom()
        {
            List<String> permissionNames = new List<String>();
            for (int i = 0; i < 8; i++)
            {
                permissionNames.Add(i.ToString());
            }
            BytePermissions permissions = new BytePermissions(permissionNames);

            byte currentPermissions = 0;
            int currentScore = 0;
            bool[] set = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                set[i] = false;
            }

            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int currentIndex = random.Next(0, 8);
                currentPermissions = permissions.SetOff(currentPermissions, permissionNames[currentIndex]);
                if (set[currentIndex])
                {
                    currentScore -= (int)Math.Pow(2, currentIndex);
                    set[currentIndex] = false;
                }

                currentIndex = random.Next(0, 8);
                currentPermissions = permissions.SetOn(currentPermissions, permissionNames[currentIndex]);
                if (!set[currentIndex])
                {
                    currentScore += (int)Math.Pow(2, currentIndex);
                    set[currentIndex] = true;
                }
                Assert.Equal((byte)currentScore, currentPermissions);
            }
        }

        [Fact]
        public void FlipHardCoded()
        {
            List<String> permissionNames = new List<String>();
            for (int i = 0; i < 8; i ++)
            {
                permissionNames.Add(i.ToString());
            }
            BytePermissions permissions = new BytePermissions(permissionNames);

            byte currentPermissions = 0;
            for (int i = 0; i < permissions.GetPermissionNames().Length; i ++)
            {
                currentPermissions = permissions.Flip(currentPermissions, i.ToString());
                Assert.Equal(currentPermissions, Math.Pow(2, i+1) - 1);
            }

            for (int i = 0; i < permissions.GetPermissionNames().Length; i ++)
            {
                currentPermissions = permissions.Flip(currentPermissions, i.ToString());
                Assert.Equal(currentPermissions, 256 - Math.Pow(2, i+1));
            }
        }

        [Fact]
        public void FlipRandom()
        {
            List<String> permissionNames = new List<String>();
            for (int i = 0; i < 8; i++)
            {
                permissionNames.Add(i.ToString());
            }
            BytePermissions permissions = new BytePermissions(permissionNames);

            byte currentPermissions = 0;
            int currentScore = 0;
            bool[] set = new bool[8];
            for (int i = 0; i < 8; i ++)
            {
                set[i] = false;
            }

            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                int currentIndex = random.Next(0, 8);
                currentPermissions = permissions.Flip(currentPermissions, permissionNames[currentIndex]);
                if (set[currentIndex])
                {
                    currentScore -= (int)Math.Pow(2, currentIndex);
                }
                else
                {
                    currentScore += (int)Math.Pow(2, currentIndex);
                }
                set[currentIndex] = !set[currentIndex];
                Assert.Equal((byte)currentScore, currentPermissions);
            }
        }

        [Fact]
        public void CheckNameRandom()
        {
            Random random = new Random();
            string[] names = new string[8];
            for (int i = 0; i < names.Length; i ++)
            {
                int nextLength = random.Next(1, 30);
                for (int j = 0; j < nextLength; j ++)
                {
                    int nextCharacter = random.Next(65, 126);
                    names[i] += (char)nextCharacter;
                }
            }
            BytePermissions permissions = new BytePermissions(new List<string>(names));
            for(int i = 0; i < 8; i ++)
            {
                string badString;
                Assert.True(permissions.CheckPermissionName(names[i]));
                do
                {
                    int modificationIndex = random.Next(0, names[i].Length);
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int before = 0; before < modificationIndex; before++)
                    {
                        stringBuilder.Append(names[i][before]);
                    }
                    int changeValue = random.Next(0, 2);
                    if (changeValue == 0)
                    {
                        changeValue = -1;
                    }
                    stringBuilder.Append((char)(names[i][modificationIndex] + changeValue));
                    for (int after = modificationIndex + 1; after < names[i].Length; after++)
                    {
                        stringBuilder.Append(names[i][after]);
                    }
                    badString = stringBuilder.ToString();
                } while (names.Contains(badString));
                Assert.False(permissions.CheckPermissionName(badString));
            }
            Assert.Equal(names, permissions.GetPermissionNames().ToArray());
        }


        [Fact]
        public void GetEnabledDisabledRandom()
        {
            List<String> permissionNames = new List<String>();
            for (int i = 0; i < 8; i++)
            {
                permissionNames.Add(i.ToString());
            }
            BytePermissions permissions = new BytePermissions(permissionNames);

            byte currentPermissions = 0;
            bool[] set = new bool[8];
            for (int i = 0; i < 8; i++)
            {
                set[i] = false;
            }

            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int currentIndex = random.Next(0, 8);
                currentPermissions = permissions.SetOff(currentPermissions, permissionNames[currentIndex]);
                if (set[currentIndex])
                {
                    set[currentIndex] = false;
                }

                currentIndex = random.Next(0, 8);
                currentPermissions = permissions.SetOn(currentPermissions, permissionNames[currentIndex]);
                if (!set[currentIndex])
                {
                    set[currentIndex] = true;
                }
                
                List<string> enabled = new List<string>();
                List<string> disabled = new List<string>();
                for (int j = 0; j < 8; j ++)
                {
                    if (set[j]) 
                    {
                        enabled.Add(j.ToString());
                    }
                    else
                    {
                        disabled.Add(j.ToString());
                    }
                }
                Assert.Equal(enabled, permissions.GetEnabledPermissions(currentPermissions));
                Assert.Equal(disabled, permissions.GetDisabledPermissions(currentPermissions));
            }
        }
    }
}