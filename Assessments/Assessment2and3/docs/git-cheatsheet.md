# Git Workflow & Command Cheatsheet

This document outlines the standard Git commands and workflows for the FileShare P2P project. 

## 1. Quick Reference: The Core Commands

### Setup & Branching
* `git checkout master` - Switch to the master branch.
* `git pull` - Fetch and merge the latest code from the remote repository.
* `git checkout -b <branch-name>` - Create and switch to a new branch.
* `git branch` - List all local branches.

### Staging & Committing
* `git status` - View modified, staged, and untracked files.
* `git add <file>` - Stage a specific file for commit.
* `git add .` - Stage all modified and new files.
* `git commit -m "<message>"` - Save staged changes to the local repository.

### Pushing & Syncing
* `git push -u origin <branch-name>` - Push a new branch to GitHub for the first time.
* `git push` - Push subsequent commits to an existing remote branch.

### Cleanup & Maintenance
* `git branch -d <branch-name>` - Delete a local branch safely.
* `git stash` - Temporarily shelf uncommitted changes.
* `git stash pop` - Restore shelved changes.
* `git commit --amend -m "<message>"` - Modify the last commit message.

---

## 2. Detailed Explanations & Project Examples

### Creating Branches
We use **Issue-Driven Development** and **dash-separated naming conventions** (`type-task-name`). Always ensure your local `master` branch is up to date before branching off.

**Example Scenario:** You are assigned to build the chunk validation logic in `FileShare.Lib`.
```bash
git checkout master
git pull
git checkout -b feature-chunk-validation
```
*Explanation:* `checkout -b` is a shortcut that creates the new branch and immediately moves you into it. If you just need to switch to an existing branch, omit the `-b`.

### Staging and Committing Work
Commits are checkpoints. They should represent a single logical update. Do not commit half-broken code.

**Example Scenario:** You added a new class and updated the XML documentation.
```bash
git status
git add FileShare.Lib/ChunkValidator.cs
git commit -m "Add core MerkleTree validation logic"
```
*Explanation:* `git status` is your best friend—run it constantly to see what you are about to commit. `git add` moves your changes into the "staging area." `git commit` permanently saves those staged changes to your local timeline. 

### Pushing to GitHub
When you are ready to back up your work or open a Pull Request (PR), you must push your local commits to the GitHub server.

**Example Scenario:** You are pushing your `feature-chunk-validation` branch for the first time.
```bash
git push -u origin feature-chunk-validation
```
*Explanation:* The `-u origin` part establishes a tracking link between your computer and GitHub. For all future commits on this specific branch, you only need to type `git push`.

### Cleaning Up After a Merge
Once your Pull Request is approved and merged into `master` on GitHub, your feature branch is no longer needed. You must delete it locally to prevent terminal clutter.

**Example Scenario:** Your chunk validation PR was just merged.
```bash
git checkout master
git pull
git branch -d feature-chunk-validation
```
*Explanation:* First, you move back to `master` and pull the newly merged code. Then, `git branch -d` deletes the old task branch from your computer. If Git warns you that the branch isn't fully merged (usually a sync delay), you can force-delete it with a capital `-D`.

### The "Stash" Command
Sometimes you are working on a feature, and the Project Manager asks you to quickly fix a bug on the `master` branch. You aren't ready to commit your current feature work yet.

**Example Scenario:** Pausing work to switch branches.
```bash
git stash
git checkout master
```
*Explanation:* `git stash` takes all your messy, uncommitted code and hides it in a temporary clipboard, returning your branch to a clean state so you can switch away. 

**Example Scenario:** Resuming your work later.
```bash
git checkout feature-chunk-validation
git stash pop
```
*Explanation:* `git stash pop` takes the code out of the temporary clipboard and pastes it exactly where you left off.
