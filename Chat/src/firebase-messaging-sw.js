importScripts(
  "https://www.gstatic.com/firebasejs/11.4.0/firebase-app-compat.js"
);
importScripts(
  "https://www.gstatic.com/firebasejs/11.4.0/firebase-messaging-compat.js"
);

const firebaseConfig = {
  apiKey: "AIzaSyBDXwMQNegWjarLOhzCKpgQPL-sVQ8RAX8",
  authDomain: "chat-73900.firebaseapp.com",
  projectId: "chat-73900",
  storageBucket: "chat-73900.firebasestorage.app",
  messagingSenderId: "677555558534",
  appId: "1:677555558534:web:c10628bb5bb4ffd7eb1907",
  measurementId: "G-NRY25QJ8TS"
};

const app = firebase.initializeApp(firebaseConfig);
const messaging = firebase.messaging();