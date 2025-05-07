# 📦 Computational Geometry — Convex Hull Algorithms (C#)

This project contains multiple classic **Convex Hull algorithms** implemented in C#. Convex Hull algorithms are essential in computational geometry for determining the smallest convex polygon enclosing a set of points.

---

## 📑 Included Algorithms

| Algorithm          | Description                                                  | Time Complexity                    |
|:------------------|:------------------------------------------------------------|:-----------------------------------|
| **DivideAndConquer** | Uses a divide-and-conquer strategy, similar to merge sort.  | **O(n log n)**                     |
| **ExtremePoints**     | Identifies points not enclosed by any triangle formed by other points. | **O(n³)**                    |
| **ExtremeSegments**   | Detects segments forming the convex hull by checking extreme connections. | **O(n³)**               |
| **GrahamScan**        | Sorts points by angle and builds the hull using a stack.   | **O(n log n)**                     |
| **Incremental**       | Adds points one at a time, updating the hull incrementally. | **O(n²)**                        |
| **JarvisMarch**       | Also known as Gift Wrapping; selects hull points one by one in a counter-clockwise order. | **O(nh)** *(h = hull points)* |
| **QuickHull**         | A QuickSort-inspired recursive approach to building the convex hull. | **O(n log n)** (avg), **O(n²)** (worst) |

---

## 📂 Project Structure

ConvexHullAlgorithms
├── DivideAndConquer.cs
├── ExtremePoints.cs
├── ExtremeSegments.cs
├── GrahamScan.cs
├── Incremental.cs
├── JarvisMarch.cs
├── QuickHull.cs
└── PolygonTriangulation


---

## 💡 Requirements

- .NET Core / .NET Framework
- Visual Studio or Visual Studio Code (recommended)

---

## 🚀 How to Run

1. Clone this repository.
2. Open the solution in Visual Studio or VS Code.
3. Build and run the desired algorithm class.

---

## 📊 Notes

- Most algorithms assume **2D Cartesian coordinates**.
- Floating-point precision might affect collinearity and point-on-line checks.
- Ideal for computational geometry teaching, research, and competitive programming.

---

## 📝 License

This project is open-source and available under the [MIT License](LICENSE).
