#!/bin/bash

HOOKS_DIR=".git/hooks"
GITHOOKS_DIR="githooks"

echo "🔧 Installation des hooks Git..."

# Vérifie que .git existe
if [ ! -d ".git" ]; then
  echo "❌ Ce répertoire ne semble pas être la racine d’un dépôt Git (pas de .git/)"
  exit 1
fi

# Crée le dossier hooks s'il n'existe pas
if [ ! -d "$HOOKS_DIR" ]; then
  echo "📁 Le dossier $HOOKS_DIR n’existe pas, création..."
  mkdir -p "$HOOKS_DIR"
fi

# Copie tous les hooks depuis githooks/ dans .git/hooks/
for hook in "$GITHOOKS_DIR"/*; do
  HOOK_NAME=$(basename "$hook")
  cp "$hook" "$HOOKS_DIR/$HOOK_NAME"
  chmod +x "$HOOKS_DIR/$HOOK_NAME"
  echo "✅ Hook $HOOK_NAME installé."
done

echo "🎉 Tous les hooks ont été installés avec succès."
