# To Do

## 2019-06-18

### MVP
1. Cross platform application
    1. Distributed in zip format
    1. Runs on
        1. Windows (10)
        2. macOS (10.14)
        3. Ubuntu (18.04)
1. Capable of being used as a simple git credential helper
1. With the following characteristics:
    1. Correctly supports the following [Environment Settings](https://github.com/microsoft/Git-Credential-Manager-for-Windows/blob/b76af7ecd788dad78bf0c30c3c9a5d1a35f16551/Docs/Environment.md):
        1. GCM_MODAL_PROMPT
        1. GCM_INTERACTIVE
        1. GCM_TRACE
        1. GCM_WRITELOG
    1. Correctly supports the following [Commands](https://github.com/microsoft/Git-Credential-Manager-for-Windows/blob/d8b3b320c253cea8594fcc3f3f57d76b0abc1d79/Docs/CredentialManager.md#get--store--erase--fill--approve--reject)
        1. GET [/]
        2. STORE [/]
        3. ERASE [/]
    1. By default supports a GUI for Basic Credential input
        1. username
        2. password (masked)
    1. Also supports a CLI for Basic Credential input
        1.username
        2. password (masked)
    1. Reads/Writes/Deletes encrypted credentials from file storage
1. Built to be extensible

## Future

### storage
1. macOS Keychain for storage
1. Windows Credential Manager/Vault for storage
1. Ubuntu Keyring for storage

### hosts
1. Bitbucket.org
1. Bitbucket Server
1. Github.com
1. GHE
1. gitlab.com
1. GL CE
1. GL Enterprise
1. Azure Devops (Cloud)
1. Azure Devops (Server)

