# 🥊 BluntBrawl

## 🛠️ Configure Local Git Hooks

In order to enforce a consistent and healthy commit process, **please run the appropriate hook setup script**:

- For **macOS/Linux** users:  
  Run `setup-hooks.sh`
- For **Windows** users:  
  Run `setup-hooks.bat`

These scripts will install local Git hooks to validate your commit messages and prevent oversized files from being committed.

---

## ✏️ Commit Message Convention

Your commit message should begin with **one of the following prefixes**, followed by a colon (`:`):


✅ Example (valid):  
`Added: Functionality works.`

❌ Example (invalid):  
`Added functionality doesn't.`

---

## 🌿 Branch Workflow

To maintain a clean and organized development flow, we are using the following branches:

### `main`
- ✅ Contains **stable, functional features**
- 🧼 Production-ready commits only

### `developpement`
- 🔧 Contains **work-in-progress features**
- 🧪 Scene editing, experimentation, and testing

---

## 🎮 Scene Workflow

Each team member should work inside their own dedicated scene:
- `Lex` works in scene `Lex`
- `Bunny` works in scene `Bunny`
- etc.

This ensures clear ownership and avoids conflicts when pushing changes.

---

## 🧩 Build Scenes

Build scenes are composed mostly of **prefabs**.  
This modular structure helps avoid merge conflicts and makes collaboration easier.

---

## 🧱 Prefab Workflow

- Create as many prefabs as needed.
- Always edit prefabs using the **Prefab Editor**, not directly in scenes.
- This helps avoid unwanted "Overrides" and ensures your changes are properly saved.

---

Feel free to update this README as the project evolves.
