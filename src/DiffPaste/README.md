# Diff Paste

**Diff Paste** is a Visual Studio Code extension that allows you to apply diff data directly to your active document. Similar to how `git diff` works, this extension interprets `-` and `+` indicators in unified diff format and accurately applies the changes to your code.

## Features

- **Apply Unified Diffs:** Seamlessly apply changes from unified diff data containing `-` (deletions) and `+` (additions).
- **Easy Access:** Execute the diff paste functionality via the context menu or using a keyboard shortcut.
- **Efficient Workflow:** Quickly integrate changes from diffs without manual editing.

## Installation

1. Open Visual Studio Code.
2. Go to the Extensions view by clicking on the Extensions icon in the Activity Bar on the side of the window or by pressing `Ctrl+Shift+X`.
3. Search for **Diff Paste**.
4. Click **Install** on the Diff Paste extension.

## Usage

1. **Copy Unified Diff Data:**
   - Ensure that your clipboard contains the unified diff data you want to apply. The diff should use `-` for deletions and `+` for additions.

2. **Apply the Diff:**
   - **Context Menu:** Right-click within the code editor and select **Diff Paste** from the context menu.
   - **Keyboard Shortcut:** Press `Ctrl+M, V` to execute the diff paste command.

3. **Result:**
   - The extension will parse the diff data and apply the changes directly to your active document, updating the content as specified.

## Example

### Test Case 1: Simple Deletion and Insertion

**Original Document:**
```
line1
line2
line3
line4
```

**Clipboard Diff:**
```
  line1
- line2
- line3
+ new_line2
+ new_line3
  line4
```

**Expected Result After Applying Diff Paste:**
```
line1
new_line2
new_line3
line4
```

### Steps to Apply the Test Case

1. **Prepare the Original Document:**
   - Open a file in VS Code containing the original document:
     ```
     line1
     line2
     line3
     line4
     ```

2. **Copy the Diff:**
   - Copy the following diff data to your clipboard:
     ```
       line1
     - line2
     - line3
     + new_line2
     + new_line3
       line4
     ```

3. **Execute Diff Paste:**
   - Right-click in the editor and select **Diff Paste** or press `Ctrl+M, V`.

4. **Verify the Changes:**
   - The document should now reflect the expected result:
     ```
     line1
     new_line2
     new_line3
     line4
     ```

## Keyboard Shortcut

- **Apply Diff Paste:** `Ctrl+M, V`

> **Note:** Ensure that the unified diff data is correctly formatted and copied to your clipboard before using the Diff Paste feature.

## License

This project is licensed under the [MIT License](LICENSE).

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any enhancements or bug fixes.