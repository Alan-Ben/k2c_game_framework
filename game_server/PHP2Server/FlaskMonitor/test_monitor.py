import unittest
from server_monitor import app

class TestMonitor(unittest.TestCase):
    def setUp(self):
        self.app = app.test_client()
        self.app.testing = True

    def test_index_route(self):
        response = self.app.get('/')
        self.assertEqual(response.status_code, 200)

    def test_status_route(self):
        response = self.app.get('/api/status')
        self.assertIsNotNone(response.json)

if __name__ == '__main__':
    unittest.main()