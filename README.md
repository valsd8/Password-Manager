
# 🔐 Password Manager

This is a simple and secure **Password Manager** built with **C# (WPF)**.  

---

## 🗄️ How is the data stored

The data is stored using **AES-256 encryption**.  
The encryption key is a **derivation** of the user's master password combined with a **unique salt**.

All data is written to a file named `encrypted.bin`, which contains a **JSON-serialized object** similar to this:

```json
{
  "Name": "My passwords",
  "IV": "iXUhN2Idla6vAyZ2aqngJQ==",
  "Salt": "b7gHxTE5O0cEwyysJ9+sMPBrqNxqWGq+t9KdZkGGyQ8=",
  "EncryptedData": "2svei9tV29wcx5lRzBJ+AKBnRu/QVLxrFSObm1Ek9AI="
}
```


![App GUI](Images/appGUI.png)

# 

