package com.web.application;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class Solution {

    public static void main(String[] args) {
        // Đường dẫn tới file .obj nguồn (chứa mô hình 3D)
        var inputFilePath = "src/main/resources/teapot.obj";
        // Thư mục lưu trữ các file .obj sau khi phân tách
        var outputFolderPath = "src/main/resources/";

        try {
            // Đọc tất cả các dòng chứa đỉnh (vertices) từ file nguồn .obj
            var vertices = readVertices(inputFilePath);
            // Đọc tất cả các dòng chứa mặt (faces) từ file nguồn .obj
            var faces = readFaces(inputFilePath);

            // Khởi tạo danh sách chứa đỉnh cho từng phần của mô hình
            var partVertices = initializePartVertices();
            // Khởi tạo bản đồ lưu trữ chỉ số gốc của các đỉnh cho từng phần của mô hình
            var partVertexIndices = initializePartVertexIndices();

            // Phân loại các đỉnh vào từng phần dựa trên tọa độ x, z của chúng
            assignVerticesToParts(vertices, partVertices, partVertexIndices);

            // Phân loại các mặt vào từng phần dựa trên các đỉnh mà chúng sử dụng
            var partFaces = splitFacesByPart(faces, partVertexIndices);

            // Lưu từng phần của mô hình vào các file .obj riêng biệt
            saveParts(outputFolderPath, partVertices, partFaces);
        } catch (IOException ignored) {
            // Nếu có lỗi trong quá trình đọc/ghi file, bỏ qua mà không xử lý
        }
    }

    static List<String> readVertices(String inputFilePath) throws IOException {
        // Tạo danh sách để chứa các dòng đỉnh (vertices)
        var vertices = new ArrayList<String>();
        // Mở file nguồn và đọc từng dòng
        try (var reader = new BufferedReader(new FileReader(inputFilePath))) {
            String line;
            // Lặp qua từng dòng của file
            while ((line = reader.readLine()) != null) {
                // Nếu dòng bắt đầu với "v ", đó là một đỉnh (vertex)
                if (line.startsWith("v ")) {
                    vertices.add(line);  // Lưu đỉnh vào danh sách vertices
                }
            }
        }
        return vertices; // Trả về danh sách chứa các đỉnh đã đọc
    }

    static List<String> readFaces(String inputFilePath) throws IOException {
        // Tạo danh sách để chứa các dòng mặt (faces)
        var faces = new ArrayList<String>();
        // Mở file nguồn và đọc từng dòng
        try (var reader = new BufferedReader(new FileReader(inputFilePath))) {
            var line = "";
            // Lặp qua từng dòng của file
            while ((line = reader.readLine()) != null) {
                // Nếu dòng bắt đầu với "f ", đó là một mặt (face)
                if (line.startsWith("f ")) {
                    faces.add(line);  // Lưu mặt vào danh sách faces
                }
            }
        }
        return faces; // Trả về danh sách chứa các mặt đã đọc
    }

    static ArrayList<ArrayList<String>> initializePartVertices() {
        // Khởi tạo danh sách các phần, mỗi phần sẽ lưu các đỉnh của nó
        var partVertices = new ArrayList<ArrayList<String>>();
        // Tạo 4 phần (theo yêu cầu) cho mô hình
        for (var i = 0; i < 4; i++) {
            partVertices.add(new ArrayList<>()); // Mỗi phần có danh sách đỉnh riêng
        }
        return partVertices; // Trả về danh sách các phần đỉnh đã khởi tạo
    }

    static ArrayList<HashMap<Integer, Integer>> initializePartVertexIndices() {
        // Khởi tạo danh sách các bản đồ cho mỗi phần
        // Mỗi bản đồ lưu các chỉ số của các đỉnh trong phần đó
        var partVertexIndices = new ArrayList<HashMap<Integer, Integer>>();
        // Tạo 4 phần và bản đồ cho mỗi phần
        for (var i = 0; i < 4; i++) {
            partVertexIndices.add(new HashMap<>()); // Bản đồ cho từng phần
        }
        return partVertexIndices; // Trả về danh sách các bản đồ chỉ số đỉnh đã khởi tạo
    }

    static void assignVerticesToParts(List<String> vertices, ArrayList<ArrayList<String>> partVertices, ArrayList<HashMap<Integer, Integer>> partVertexIndices) {
        // Lặp qua tất cả các đỉnh để phân loại chúng vào các phần tương ứng
        for (int i = 0; i < vertices.size(); i++) {
            var vertex = vertices.get(i); // Lấy đỉnh tại vị trí i
            var parts = vertex.split(" "); // Tách các thành phần của đỉnh theo dấu cách
            var x = Double.parseDouble(parts[1]); // Lấy tọa độ x của đỉnh
            var z = Double.parseDouble(parts[3]); // Lấy tọa độ z của đỉnh

            // Xác định phần của đỉnh dựa trên tọa độ x, z
            var partIndex = getPartIndex(x, z);
            // Thêm đỉnh vào danh sách của phần tương ứng
            partVertices.get(partIndex).add(vertex);
            // Lưu chỉ số gốc của đỉnh và vị trí của nó trong phần
            partVertexIndices.get(partIndex).put(i + 1, partVertices.get(partIndex).size());
        }
    }

    static ArrayList<ArrayList<String>> splitFacesByPart(List<String> faces, ArrayList<HashMap<Integer, Integer>> partVertexIndices) {
        // Khởi tạo danh sách các mặt cho mỗi phần
        var partFaces = initializePartFaces();
        // Duyệt qua tất cả các mặt và phân loại vào các phần tương ứng
        for (var face : faces) {
            var faceParts = face.split(" "); // Tách các thành phần của mặt theo dấu cách
            var vertexIndices = extractVertexIndices(faceParts); // Lấy danh sách chỉ số của các đỉnh trong mặt

            // Xác định phần mà mặt này thuộc về (dựa trên các đỉnh)
            var partIndex = getFacePartIndex(vertexIndices, partVertexIndices);
            // Nếu xác định được phần, thêm mặt vào phần tương ứng
            if (partIndex != -1) {
                var newFace = createNewFace(vertexIndices, partVertexIndices.get(partIndex)); // Tạo mặt mới với chỉ số đỉnh đã cập nhật
                partFaces.get(partIndex).add(newFace); // Thêm mặt vào phần tương ứng
            }
        }
        return partFaces; // Trả về danh sách các phần mặt đã phân loại
    }

    static ArrayList<ArrayList<String>> initializePartFaces() {
        // Khởi tạo danh sách chứa các mặt cho mỗi phần
        var partFaces = new ArrayList<ArrayList<String>>();
        // Tạo 4 phần, mỗi phần sẽ chứa các mặt của nó
        for (var i = 0; i < 4; i++) {
            partFaces.add(new ArrayList<>()); // Thêm một danh sách cho các mặt của từng phần
        }
        return partFaces; // Trả về danh sách các phần mặt đã khởi tạo
    }

    static int[] extractVertexIndices(String[] faceParts) {
        // Lấy danh sách chỉ số đỉnh từ một dòng mặt
        var vertexIndices = new int[faceParts.length - 1];
        for (var j = 1; j < faceParts.length; j++) {
            vertexIndices[j - 1] = Integer.parseInt(faceParts[j].split("/")[0]); // Chỉ lấy chỉ số của đỉnh
        }
        return vertexIndices; // Trả về mảng chứa chỉ số đỉnh
    }

    static int getFacePartIndex(int[] vertexIndices, ArrayList<HashMap<Integer, Integer>> partVertexIndices) {
        var partIndex = -1; // Phần chưa được xác định
        for (var vertexIndex : vertexIndices) {
            for (var k = 0; k < 4; k++) {
                // Nếu phần k chứa chỉ số của đỉnh, xác định phần
                if (partVertexIndices.get(k).containsKey(vertexIndex)) {
                    if (partIndex == -1) {
                        partIndex = k; // Xác định phần đầu tiên chứa đỉnh này
                    } else if (partIndex != k) {
                        return -1; // Nếu đỉnh thuộc nhiều phần, trả về -1 (mặt không thuộc một phần duy nhất)
                    }
                }
            }
        }
        return partIndex; // Trả về phần của mặt (hoặc -1 nếu mặt không thuộc một phần duy nhất)
    }

    static String createNewFace(int[] vertexIndices, HashMap<Integer, Integer> vertexIndexMap) {
        var newFace = new StringBuilder("f"); // Bắt đầu dòng mặt mới
        for (var vertexIndex : vertexIndices) {
            // Lấy chỉ số đỉnh đã được cập nhật cho phần
            var newIndex = vertexIndexMap.get(vertexIndex);
            newFace.append(" ").append(newIndex); // Thêm chỉ số vào dòng mặt
        }
        return newFace.toString(); // Trả về dòng mặt mới
    }

    static void saveParts(String outputFolderPath, ArrayList<ArrayList<String>> partVertices, ArrayList<ArrayList<String>> partFaces) throws IOException {
        // Lưu mỗi phần vào một file .obj riêng biệt
        for (var i = 0; i < 4; i++) {
            saveObjFile(outputFolderPath + "file" + (i + 1) + ".obj", partVertices.get(i), partFaces.get(i));
        }
    }

    static int getPartIndex(double x, double z) {
        // Xác định phần dựa trên tọa độ x và z
        if (x >= 0 && z >= 0) return 0;
        if (x < 0 && z >= 0) return 1;
        if (x >= 0 && z < 0) return 2;
        return 3;
    }

    static void saveObjFile(String filePath, List<String> vertices, List<String> faces) throws IOException {
        // Lưu danh sách đỉnh và mặt vào file .obj
        try (var writer = new BufferedWriter(new FileWriter(filePath))) {
            for (var vertex : vertices) {
                writer.write(vertex);
                writer.newLine();
            }
            for (var face : faces) {
                writer.write(face);
                writer.newLine();
            }
        }
    }

}
