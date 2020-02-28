import socket


controller = '00:15:83:12:12:E7'  # <adapter address>
device = '00:19:09:03:5A:AE'  # <device address>


def conn():
    sock = socket.socket(socket.AF_BLUETOOTH,
                         socket.SOCK_STREAM, socket.BTPROTO_RFCOMM)
    sock.bind((controller, 1))
    sock.connect((device, 1))
    # If not pinned it will ask you. You can use/adapt the bluez simple-agent for headless pinning
    return sock


def main():
    sock = conn()
    sock.send(b'+')
    print("Receiving...")
    while True:
        print(sock.recv(1))
    sock.close()


if __name__ == "__main__":
    main()
