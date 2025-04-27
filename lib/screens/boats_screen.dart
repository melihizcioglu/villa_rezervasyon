import 'package:flutter/material.dart';
import 'login_screen.dart';

class BoatsScreen extends StatelessWidget {
  const BoatsScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Tekne Rezervasyon'),
        actions: [
          IconButton(
            icon: const Icon(Icons.person),
            onPressed: () {
              showDialog(
                context: context,
                builder: (context) => const LoginScreen(),
              );
            },
          ),
        ],
      ),
      drawer: Drawer(
        child: ListView(
          padding: EdgeInsets.zero,
          children: [
            const DrawerHeader(
              decoration: BoxDecoration(
                color: Colors.blue,
              ),
              child: Text(
                'Kategoriler',
                style: TextStyle(
                  color: Colors.white,
                  fontSize: 24,
                ),
              ),
            ),
            ListTile(
              leading: const Icon(Icons.villa),
              title: const Text('Villalar'),
              onTap: () {
                Navigator.pop(context);
                Navigator.pushReplacementNamed(context, '/');
              },
            ),
            ListTile(
              leading: const Icon(Icons.directions_boat),
              title: const Text('Tekneler'),
              onTap: () {
                Navigator.pop(context);
              },
            ),
          ],
        ),
      ),
      body: ListView.builder(
        itemCount: 10,
        itemBuilder: (context, index) {
          return Card(
            margin: const EdgeInsets.all(8.0),
            child: ListTile(
              leading: const Icon(Icons.directions_boat, size: 40),
              title: Text('Tekne ${index + 1}'),
              subtitle: const Text('Muğla, Bodrum'),
              trailing: const Icon(Icons.arrow_forward_ios),
              onTap: () {
                // TODO: Tekne detay sayfasına yönlendir
              },
            ),
          );
        },
      ),
    );
  }
} 