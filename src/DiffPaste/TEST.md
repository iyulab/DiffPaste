# Diff Paste

`git diff`와 유사하게 `-`, `+` 표시된 diff 데이터를 현재 활성 문서에 정확히 적용할 수 있습니다.


#### 테스트 케이스 1: 단순한 삭제 및 삽입

**원본 문서:**
```
line1
line2
line3
line4
```

**클립보드 Diff:**
```
 line1
-line2
-line3
+new_line2
+new_line3
 line4
```

**기대 결과:**
```
line1
new_line2
new_line3
line4
```

#### 테스트 케이스 2: 여러 Hunks (다중 블록 변경)

**원본 문서:**
```
line1
line2
line3
line4
line5
line6
```

**클립보드 Diff:**
```
 line1
-line2
+new_line2
 line3
 line4
-line5
+new_line5
 line6
```

**기대 결과:**
```
line1
new_line2
line3
line4
new_line5
line6
```

#### 테스트 케이스 3: 삽입만 있는 경우

**원본 문서:**
```
line1
line2
line3
```

**클립보드 Diff:**
```
 line1
+line1.5
 line2
 line3
```

**기대 결과:**
```
line1
line1.5
line2
line3
```

#### 테스트 케이스 4: 삭제만 있는 경우

**원본 문서:**
```
line1
line2
line3
```

**클립보드 Diff:**
```
 line1
-line2
 line3
```

**기대 결과:**
```
line1
line3
```

#### 테스트 케이스 5: 중간 삽입 및 삭제

**원본 문서:**
```
line1
line2
line3
line4
line5
```

**클립보드 Diff:**
```
 line1
 line2
-line3
+new_line3
+new_line4.1
 line4
 line5
```

**기대 결과:**
```
line1
line2
new_line3
new_line4.1
line4
line5
```

#### 테스트 케이스 6: 복잡한 변경 (여러 삽입과 삭제)

**원본 문서:**
```
header1
line1
line2
line3
line4
footer1
footer2
```

**클립보드 Diff:**
```
 header1
-line1
+new_line1
 line2
-line3
+new_line3
+added_line3.1
 line4
 footer1
-footer2
+footer2_modified
```

**기대 결과:**
```
header1
new_line1
line2
new_line3
added_line3.1
line4
footer1
footer2_modified
```

### 테스트 케이스 7: C# 함수 변경

**원본 문서:**
```csharp
public void FunctionA()
{
    Console.WriteLine("Original Line 1");
    Console.WriteLine("Original Line 2");
}
```

**클립보드 Diff:**
```
 public void FunctionA()
 {
-    Console.WriteLine("Original Line 1");
-    Console.WriteLine("Original Line 2");
+    Console.WriteLine("Updated Line 1");
+    Console.WriteLine("Updated Line 2");
 }
```

**기대 결과:**
```csharp
public void FunctionA()
{
    Console.WriteLine("Updated Line 1");
    Console.WriteLine("Updated Line 2");
}
```

---

### 테스트 케이스 8: JavaScript 함수에 새 코드 추가

**원본 문서:**
```javascript
function greet(name) {
    console.log("Hello, " + name);
    console.log("Have a great day!");
}
```

**클립보드 Diff:**
```
 function greet(name) {
     console.log("Hello, " + name);
+    console.log("This is an additional line.");
     console.log("Have a great day!");
 }
```

**기대 결과:**
```javascript
function greet(name) {
    console.log("Hello, " + name);
    console.log("This is an additional line.");
    console.log("Have a great day!");
}
```

---

### 테스트 케이스 9: 긴 개행 포함한 텍스트 변경

**원본 문서:**
```
This is the first paragraph.

This is the second paragraph.

This is the third paragraph.
```

**클립보드 Diff:**
```
 This is the first paragraph.
 
-This is the second paragraph.
+This is the updated second paragraph.
 
 This is the third paragraph.
```

**기대 결과:**
```
This is the first paragraph.

This is the updated second paragraph.

This is the third paragraph.
```

---

### 테스트 케이스 10: HTML 문서 내 변경

**원본 문서:**
```html
<html>
    <body>
        <h1>Title</h1>
        <p>This is a paragraph.</p>
        <footer>Original Footer</footer>
    </body>
</html>
```

**클립보드 Diff:**
```
 <html>
     <body>
         <h1>Title</h1>
         <p>This is a paragraph.</p>
-        <footer>Original Footer</footer>
+        <footer>Updated Footer</footer>
     </body>
 </html>
```

**기대 결과:**
```html
<html>
    <body>
        <h1>Title</h1>
        <p>This is a paragraph.</p>
        <footer>Updated Footer</footer>
    </body>
</html>
```

### 테스트 케이스 11: JSON 데이터 수정

**원본 문서:**
```json
{
    "name": "John",
    "age": 30,
    "city": "New York"
}
```

**클립보드 Diff:**
```
 {
     "name": "John",
-    "age": 30,
+    "age": 31,
     "city": "New York"
 }
```

**기대 결과:**
```json
{
    "name": "John",
    "age": 31,
    "city": "New York"
}
```

### 테스트 케이스 12: Python 함수 수정

**원본 문서:**
```python
def calculate(a, b):
    return a + b
```

**클립보드 Diff:**
```
 def calculate(a, b):
-    return a + b
+    return a * b
```

**기대 결과:**
```python
def calculate(a, b):
    return a * b
```

### 테스트 케이스 13: 다중 블록 변경 (복잡한 코드)

**원본 문서:**
```python
def function_one():
    print("Function one start")
    print("Function one end")

def function_two():
    print("Function two start")
    print("Function two end")
```

**클립보드 Diff:**
```
 def function_one():
     print("Function one start")
-    print("Function one end")
+    print("Function one modified end")

 def function_two():
     print("Function two start")
-    print("Function two end")
+    print("Function two modified end")
+    print("Additional line for function two")
```

**기대 결과:**
```python
def function_one():
    print("Function one start")
    print("Function one modified end")

def function_two():
    print("Function two start")
    print("Function two modified end")
    print("Additional line for function two")
```

### 테스트 케이스 14: 긴 원본 코드와 코드 생략 포함한 Diff

**원본 문서:**
```python
def process_data(data):
    print("Starting data processing")
    # Step 1: Clean the data
    cleaned_data = [item.strip() for item in data if item]
    print("Data cleaned")
    
    # Step 2: Transform the data
    transformed_data = [item.upper() for item in cleaned_data]
    print("Data transformed")
    
    # Step 3: Analyze the data
    summary = {
        "count": len(transformed_data),
        "first_item": transformed_data[0] if transformed_data else None,
        "last_item": transformed_data[-1] if transformed_data else None,
    }
    print("Data analyzed")
    
    # Step 4: Save the results
    with open("output.txt", "w") as file:
        file.write("\n".join(transformed_data))
    print("Results saved")
    
    return summary

def helper_function_1():
    print("Helper function 1")

def helper_function_2():
    print("Helper function 2")

def main():
    data = [" item1 ", " item2 ", "item3", None, " item4 "]
    result = process_data(data)
    print(f"Process completed. Summary: {result}")
```

**클립보드 Diff:**
```
 def process_data(data):
     print("Starting data processing")
     # Step 1: Clean the data
     cleaned_data = [item.strip() for item in data if item]
     print("Data cleaned")
     
-    # Step 2: Transform the data
-    transformed_data = [item.upper() for item in cleaned_data]
-    print("Data transformed")
+    # Step 2: Transform and filter the data
+    transformed_data = [item.upper() for item in cleaned_data if len(item) > 4]
+    print("Data transformed and filtered")
     
     ... (코드생략)
     
     # Step 4: Save the results
-    with open("output.txt", "w") as file:
-        file.write("\n".join(transformed_data))
-    print("Results saved")
+    # Code removed for file saving (external module used)
     
     return summary

...

 def main():
     data = [" item1 ", " item2 ", "item3", None, " item4 "]
-    result = process_data(data)
-    print(f"Process completed. Summary: {result}")
+    # Processing is logged externally, summary is returned for testing
+    return process_data(data)
```

**기대 결과:**
```python
def process_data(data):
    print("Starting data processing")
    # Step 1: Clean the data
    cleaned_data = [item.strip() for item in data if item]
    print("Data cleaned")
    
    # Step 2: Transform and filter the data
    transformed_data = [item.upper() for item in cleaned_data if len(item) > 4]
    print("Data transformed and filtered")
    
    # Step 3: Analyze the data
    summary = {
        "count": len(transformed_data),
        "first_item": transformed_data[0] if transformed_data else None,
        "last_item": transformed_data[-1] if transformed_data else None,
    }
    print("Data analyzed")
    
    # Code removed for file saving (external module used)
    
    return summary

...

def main():
    data = [" item1 ", " item2 ", "item3", None, " item4 "]
    # Processing is logged externally, summary is returned for testing
    return process_data(data)
```

### 테스트 라인 추측 #1

**원본 문서:**
```
line1
line2
line1
line2
line3
```

**클립보드 Diff:**
```
 line1
-line2
 line3
```

**기대 결과:**
```
line1
line2
line1
line3
```

