import * as React from "react";
import { Link } from "react-router-dom";
import { createOrder, getOrders, Order } from "~/api";
import { OrderComponent } from "./OrderComponent";
import { OrderEditor } from "./OrderEditor";

export function Reports() {
  const [orders, setOrders] = React.useState<undefined | Order[]>(undefined);

  React.useEffect(() => {
    const abortController = new AbortController();

    async function loadOrders() {
      let orders = await getOrders(abortController.signal);
      setOrders(
        orders.sort((a, b) => {
          const timeA = Date.parse(a.createdAt);
          const timeB = Date.parse(b.createdAt);

          return timeB - timeA;
        })
      );
    }

    loadOrders();

    return () => {
      abortController.abort();
    };
  }, []);

  return (
    <div className="row g-0">
      <div className="col p-4">
        <div className="d-flex flex-row align-items-center mb-4">
          {!orders && <b className="text-muted">{"Loading..."}</b>}
          {orders && (
            <b className="text-muted">{`${orders?.length} Order(s)`}</b>
          )}
          <Link
            className="btn btn-sm btn-primary ms-auto d-xl-none"
            to="/dashboard/reports/new-order"
          >
            <i className="fas fa-plus me-2"></i>
            <span>{"New order"}</span>
          </Link>
        </div>
        <div className="row">
          {orders && (
            <React.Fragment>
              {orders.map((order) => (
                <div key={order.id} className="mb-2">
                  <OrderComponent order={order} />
                </div>
              ))}
            </React.Fragment>
          )}
        </div>
      </div>
      <div className="col-3 d-none d-xl-block border-start vh-100 bg-light p-4 sticky-top overflow-auto">
        <div className="d-flex flex-row align-items-center mb-4">
          <b className="text-muted">{"Create a new Order"}</b>
        </div>
        <div className="row">
          <OrderEditor
            onSubmit={async (order) => {
              const newOrder = await createOrder(order);
              setOrders([newOrder].concat(orders ?? []));
            }}
          />
        </div>
      </div>
    </div>
  );
}
