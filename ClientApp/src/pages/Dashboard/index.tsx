import * as React from "react";
import { Link, Route, Switch, useHistory, useLocation } from "react-router-dom";
import { createOrder, createProduct, getProduct, patchProduct, Product } from "~/api";
import { Async, AsyncResult } from "~/components/Async";
import { logoutActionAsync, useDispatch, useSelector } from "~/store";
import { OrderEditor } from "./components/OrderEditor";
import { ProductEditor } from "./components/ProductEditor";
import { Products } from "./components/Products";
import { Reports } from "./components/Reports";
import Sidebar, { SidebarItem } from "./components/Sidebar";

export default function Dashboard() {
  const history = useHistory();
  const location = useLocation();
  const dispatch = useDispatch();
  const user = useSelector((state) => state.user);

  const paneContent = (
    <div className="row g-0 h-100 bg-dark border-right text-light">
      <div className="col d-flex flex-column justify-content-between">
        <div className="row g-0 d-flex flex-column py-4">
          <div className="px-3 mb-2">
            <h5>
              <b>{"MeuEstoque"}</b>
            </h5>
          </div>
          <Link to="/dashboard/products">
            <SidebarItem
              selected={location.pathname.startsWith("/dashboard/products")}
            >
              <i className="fas fa-box me-3"></i>
              <span>{"Products"}</span>
            </SidebarItem>
          </Link>
          <Link to="/dashboard/reports">
            <SidebarItem
              selected={location.pathname.startsWith("/dashboard/reports")}
            >
              <i className="fas fa-chart-bar me-3"></i>
              <span>{"Reports"}</span>
            </SidebarItem>
          </Link>
        </div>
        <div className="row g-0 bg-dark-light p-3">
          <div className="col text-truncate">
            <small>{user?.name}</small>
          </div>
          <div
            className="col-auto ms-4"
            role="button"
            onClick={() => dispatch(logoutActionAsync())}
          >
            <span className="fas fa-sign-out-alt text-light"></span>
          </div>
        </div>
      </div>
    </div>
  );

  return (
    <div className="vh-100">
      <Sidebar content={paneContent}>
        <Switch>
          <Route exact path="/dashboard/products">
            <Products />
          </Route>
          <Route exact path="/dashboard/products/new">
            <div className="p-4">
              <ProductEditor
                onSubmit={async (product) => {
                  await createProduct(product);
                  history.push("/dashboard/products");
                }}
              />
            </div>
          </Route>
          <Route exact path="/dashboard/products/:id/edit">
            {({ match }) =>
              match && (
                <Async func={(signal) => getProduct(match.params.id, signal)}>
                  {(result: AsyncResult<Product, any>) => {
                    switch (result.state) {
                      case "complete":
                        return (
                          <div className="p-4">
                            <ProductEditor
                              defaultProduct={result.value}
                              onSubmit={async (product) => {
                                await patchProduct(result.value.id, {
                                  ...result.value,
                                  ...product,
                                });

                                history.push("/dashboard/products");
                              }}
                            />
                          </div>
                        );

                      case "error":
                        return <div>{"Error"}</div>;

                      default:
                        return <div>{"Loading..."}</div>;
                    }
                  }}
                </Async>
              )
            }
          </Route>
          <Route exact path="/dashboard/reports">
            <Reports />
          </Route>
          <Route exact path="/dashboard/reports/new-order">
            <div className="p-4">
              <OrderEditor
                onSubmit={async (order) => {
                  await createOrder(order);
                  history.push("/dashboard/reports");
                }}
              />
            </div>
          </Route>
        </Switch>
      </Sidebar>
    </div>
  );
}
